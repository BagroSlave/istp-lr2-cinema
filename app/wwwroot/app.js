// Фронтенд на чистому JavaScript: виклик Web API через fetch.
const API = '/api';

let directors = [];
let selectedYear = new Date().getFullYear();

// Гарний селектор року: поле з випадаючою сіткою років.
function buildYearPicker() {
    const btn = document.getElementById('yearBtn');
    const val = document.getElementById('yearVal');
    const pop = document.getElementById('yearPop');
    const now = new Date().getFullYear();
    pop.innerHTML = '';
    for (let y = now + 4; y >= 1960; y--) {
        const b = document.createElement('button');
        b.type = 'button';
        b.textContent = y;
        if (y === selectedYear) b.classList.add('sel');
        b.addEventListener('click', () => {
            selectedYear = y;
            val.textContent = y;
            pop.querySelectorAll('button').forEach(x => x.classList.remove('sel'));
            b.classList.add('sel');
            pop.classList.remove('open');
        });
        pop.appendChild(b);
    }
    val.textContent = selectedYear;
    btn.addEventListener('click', (e) => {
        e.stopPropagation();
        pop.classList.toggle('open');
        const sel = pop.querySelector('.sel');
        if (sel) sel.scrollIntoView({ block: 'center' });
    });
    pop.addEventListener('click', (e) => e.stopPropagation());
    document.addEventListener('click', () => pop.classList.remove('open'));
}

// Зчитати зображення з файлу, зменшити та повернути base64 data-URI.
function fileToDataUrl(file, maxW = 320) {
    return new Promise((resolve) => {
        if (!file) { resolve(null); return; }
        const reader = new FileReader();
        reader.onload = e => {
            const img = new Image();
            img.onload = () => {
                const scale = Math.min(1, maxW / img.width);
                const c = document.createElement('canvas');
                c.width = img.width * scale;
                c.height = img.height * scale;
                c.getContext('2d').drawImage(img, 0, 0, c.width, c.height);
                resolve(c.toDataURL('image/jpeg', 0.8));
            };
            img.src = e.target.result;
        };
        reader.readAsDataURL(file);
    });
}

async function loadDirectors() {
    const res = await fetch(`${API}/directors`);
    directors = await res.json();
    const dl = document.getElementById('directorList');
    dl.innerHTML = '';
    directors.forEach(d => {
        const opt = document.createElement('option');
        opt.value = d.name;
        dl.appendChild(opt);
    });
}

function directorName(id) {
    const d = directors.find(x => x.id === id);
    return d ? d.name : '—';
}

async function loadMovies() {
    const res = await fetch(`${API}/movies`);
    const movies = await res.json();
    const grid = document.getElementById('movies');
    grid.innerHTML = '';
    if (!movies.length) {
        grid.innerHTML = '<p style="color:#9aa0c0">Фільмів ще немає — додайте перший.</p>';
        return;
    }
    movies.forEach(m => {
        const poster = m.posterUrl ? `<img src="${m.posterUrl}" alt="постер">` : '🎬';
        const card = document.createElement('div');
        card.className = 'movie';
        card.innerHTML = `
            <button class="del" title="Видалити" data-id="${m.id}">×</button>
            <div class="poster">${poster}</div>
            <h3>${m.title}</h3>
            <div class="meta">${m.year} · ${directorName(m.directorId)}</div>
            <span class="rating">★ ${m.rating}</span>`;
        card.querySelector('.del').addEventListener('click', () => deleteMovie(m.id));
        grid.appendChild(card);
    });
}

async function addMovie(event) {
    event.preventDefault();
    const posterFile = document.getElementById('poster').files[0];
    const posterUrl = await fileToDataUrl(posterFile);
    const movie = {
        title: document.getElementById('title').value,
        year: selectedYear,
        rating: parseFloat(document.getElementById('rating').value),
        directorName: document.getElementById('directorName').value,
        posterUrl: posterUrl
    };
    await fetch(`${API}/movies`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(movie)
    });
    document.getElementById('title').value = '';
    document.getElementById('directorName').value = '';
    document.getElementById('poster').value = '';
    await loadDirectors();
    await loadMovies();
}

async function deleteMovie(id) {
    await fetch(`${API}/movies/${id}`, { method: 'DELETE' });
    await loadMovies();
}

document.getElementById('addForm').addEventListener('submit', addMovie);

(async function init() {
    buildYearPicker();
    await loadDirectors();
    await loadMovies();
})();
