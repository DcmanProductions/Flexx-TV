if(!window.location.hash)window.location.href = "/Library/Movies/";
const id = window.location.hash.replace("#", "");
const returnAddress = `/Library/Movies/View/#${id}`;

video.src = `${server}/api/streaming/${id}/trailer`;

video.addEventListener("ended", () => { window.location.href = returnAddress; });
document.getElementById("closeBtn").addEventListener("click", () => { window.location.href = returnAddress; });


var movie = {};
fetch(`${server}/api/streaming/movies/get/${id}`)
    .then(response => response.json())
    .then((data) => {
        movie = data["items"];
    }).then(() => {
        document.getElementById("videoTitle").innerHTML = movie["name"] + document.getElementById("videoTitle").innerHTML;
        document.getElementById("videoYear").innerHTML = "Trailer";
    });