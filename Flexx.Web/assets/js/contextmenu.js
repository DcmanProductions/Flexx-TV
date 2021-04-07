var cm = document.querySelector('.custom-cm');
var isMenuOpen = false;
var defMenu = cm.innerHTML;
window.addEventListener("contextmenu", event => {
    event.preventDefault();
    showContextMenu();
    cm.style.top = `${(event.y + cm.offsetHeight) + window.scrollY >= window.innerHeight + 10 + window.scrollY ? window.innerHeight - cm.offsetHeight - 10 + window.scrollY : event.y + window.scrollY}px`;
    cm.style.left = `${event.x + cm.offsetWidth >= window.innerWidth + 10 ? window.innerWidth - cm.offsetWidth - 10 : event.x}px`;
});

window.addEventListener("blur", () => showContextMenu(false));

window.addEventListener("click", e => {
    if (isMenuOpen) showContextMenu(false)
})

function showContextMenu(show = true) {
    cm.style.display = show ? "block" : "none";
    cm.style.top = "";
    cm.style.left = "";
    isMenuOpen = show;
}

function clearContextMenu() {
    cm.innerHTML = "";
}

function loadContextMenuItems(html) {
    cm.innerHTML = html;
}

function addContextMenuItem(html) {
    cm.innerHTML += html;
}

function loadDefaultContextMenu() {
    cm.innerHTML = defMenu;
}