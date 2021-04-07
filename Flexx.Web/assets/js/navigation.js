function changeActiveNavItem(id) {
    var items = document.getElementsByClassName('navItem');
    Array.from(items).forEach(i => {
        i.classList.remove('active');
    })
    if (id != -1)
        items[id].classList.add('active');
}