document.title = `${document.title} - FlexxTV`
let uri = document.baseURI.replaceAll(document.baseURI.split('/')[document.baseURI.split('/').length - 1], "")+window.location.hash;
window.history.pushState("", "", uri);
Array.from(document.getElementsByClassName("builder")).forEach(item => {
    let id = item.id;
    let builder = item.dataset.builder;
    $(`#${id}`).load(`/Pages/builder/${builder}.html`);
});