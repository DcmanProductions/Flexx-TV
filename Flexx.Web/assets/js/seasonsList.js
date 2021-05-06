var test = {};
document.querySelector('body').style.background = "black";
document.getElementById("mediaView").innerHTML = "<h3>Searching for TV Shows...</h3>"
var html = "<ul>";

var throbber = document.createElement("div")
throbber.className = "spinner"
throbber.id = "loading"
throbber.style.width = "64px"
throbber.style.height = "64px"
throbber.style.position = "fixed"
throbber.style.left = "50%"
throbber.style.top = "50%"
throbber.style.zIndex = "999"

var background = document.createElement("div")
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

var timer = setInterval(() => {
    if (userName != "") {
        let id = window.location.hash.replace("#", "");
        let url = `${server}/api/streaming/tv/${userName}/get/${id}/seasons`
        fetch(url)
            .then(response => response.json())
            .then((data) => {
                test = data;
            }).then(() => {
                test["items"].forEach(item => {
                    let watched = false;
                    watched = item["watched"];
                    watchedPercentage = item["watchedPercentage"];
                    let cName = watched ? "" : "unwatched"
                    html += `
                            <li>
                            <div class="mediaItem ${cName}" style="position: relative" data-movie-name="" data-movie-percentage=0 id="" data-movie-watched=false onclick="window.location.href = '/Library/TV/Seasons/Episodes#${id}-${item["number"]}'">
                            <img src="${item["posterURL"]}" />
                            <div class="movieTitle">
                            ${item["name"]}
                            </div>
                            </div>
                            </li>
                            `;
                })
            }).then(() => {
                html += "</ul>";
            }).then(() => {
                document.getElementById("mediaView").innerHTML = html;
                document.querySelector('body').style.backgroundRepeat = "no-repeat"
                document.querySelector('body').style.backgroundSize = "cover"
                document.querySelector('body').style.backgroundAttachment = "fixed"
            })
            .catch((err) => {
                console.log(err)
                document.getElementById("mediaView").innerHTML = "<h3>Unable to Connect to Server!</h3>"
            });

        document.getElementById("movie").style.display = ""
        document.getElementById("loading").remove();
        document.getElementById("background").remove();
        clearInterval(timer)
    }
}, 100)