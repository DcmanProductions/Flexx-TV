import("/assets/js/user.js")


let throbber = document.createElement("div")
throbber.className = "spinner"
throbber.id = "loading"
throbber.style.width = "64px"
throbber.style.height = "64px"
throbber.style.position = "fixed"
throbber.style.left = "50%"
throbber.style.top = "50%"
throbber.style.zIndex = "999"

let background = document.createElement("div")
background.id = "background"
background.style.position = "fixed"
background.style.background = "black"
background.style.width = "100%"
background.style.height = "100%"
background.style.top = "0"
background.style.left = "0"
background.appendChild(throbber);
document.body.appendChild(background);
document.getElementById("movie").style.display = "none"

let timer = setInterval(() => {
    if (userName != "") {
        if (window.location.hash) {
            var id = window.location.hash.replace("#", "").split('-')[0].replace("-", "");
            var season = window.location.hash.replace("#", "").split('-')[1].replace("-", "");
            var episode = window.location.hash.replace("#", "").split('-')[2].replace("-", "");
            var movie = {};
            fetch(`${server}/api/streaming/tv/${userName}/get/${id}/${season}/${episode}`)
                .then(response => response.json())
                .then((data) => {
                    movie = data["items"];
                    console.log(`${server}/api/streaming/tv/${userName}/get/${id}/${season}/${episode}`)
                }).then(() => {
                    // Poster Image
                    document.getElementById("moviePoster").style.background = `url(${movie["showPosterURL"]})`;
                    document.getElementById("moviePoster").style.backgroundRepeat = "no-repeat";
                    document.getElementById("moviePoster").style.backgroundSize = "cover";

                    // Background Image
                    document.querySelector("body").style.background = `url(${movie["showCoverURL"]})`;
                    document.querySelector("body").style.backgroundRepeat = "no-repeat";
                    document.querySelector("body").style.backgroundSize = "cover";
                    document.querySelector("body").style.backgroundAttachment = "fixed";

                    document.querySelector('title').text = `${movie["name"]} - Flexx TV`;

                    document.getElementById("movieTitle").innerHTML = movie["name"];
                    document.getElementById("movieSummery").innerHTML = movie["summery"];
                    document.getElementById("rating").innerHTML = movie["mpaa"];
                    document.getElementById("resolution").innerHTML = movie["resolution"];
                    document.getElementById("length").innerHTML = movie["duration"];
                    document.getElementById("language").innerHTML = movie["language"];
                    document.querySelector(".mediaPosterProgressIndicator_XL").style.width = `${movie["watchedPercentage"]}%`


                }).then(() => {
                    document.getElementById("playBtn").addEventListener("click", () => {
                        window.location.href = `/Library/Movies/Watch/#${id}`;
                    });
                    document.getElementById("backBtn").addEventListener("click", () => {
                        window.location.href = `/Library/TV/Seasons/Episodes#${id}-${season}`;
                    });
                    document.getElementById("secPlayBtn").addEventListener("click", () => {
                        window.location.href = `/Library/Movies/Watch/#${id}`;
                    });
                })
                .catch((err) => {
                    console.log(err);
                    // window.location.href = "/Library/TV/";
                });
        } else {
            // window.location.href = "/Library/TV/";
        }
        document.getElementById("movie").style.display = ""
        document.getElementById("loading").remove();
        document.getElementById("background").remove();
        clearInterval(timer);
    }
}, 100)