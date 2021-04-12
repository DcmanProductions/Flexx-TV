var test = {};
document.querySelector('body').style.background = "black";
document.getElementById("mediaView").innerHTML = "<h3>Searching for TV Shows...</h3>"
var html = "<ul>";
var timer = setInterval(() => {
    if (userName != "") {
        let url = `${server}/api/streaming/tv/${userName}/get/`
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
                            <div class="mediaItem ${cName}" style="position: relative" data-movie-name="" data-movie-percentage=0 id="" data-movie-watched=false  onmouseover="document.querySelector('body').style.background = 'url(${item["coverURL"]})'" onclick="window.location.href = '/Library/TV/Seasons#${item["id"]}'">
                            <img src="${item["posterURL"]}" />
                            <div class="movieTitle">
                            ${item["name"]}
                            <div class="movieYear">
                            ${item["year"]}
                            </div>
                            </div>
                            </div>
                            </li>
                            `;
                })
            }).then(() => {
                html += "</ul>";
            }).then(() => {
                document.getElementById("mediaView").innerHTML = html;
                document.querySelector('body').style.background = `url(${test["items"][0]["coverURL"]})`;
                document.querySelector('body').style.backgroundRepeat = "no-repeat"
                document.querySelector('body').style.backgroundSize = "cover"
                document.querySelector('body').style.backgroundAttachment = "fixed"
            }).then(() => {
                Array.from(document.getElementsByClassName('mediaItem')).forEach(item => {
                    item.addEventListener('mouseover', e => {
                        document.querySelector('body').style.backgroundRepeat = "no-repeat"
                        document.querySelector('body').style.backgroundSize = "cover"
                        document.querySelector('body').style.backgroundAttachment = "fixed"
                    })
                })
            })
            .catch((err) => {
                console.log(err)
                document.getElementById("mediaView").innerHTML = "<h3>Unable to Connect to Server!</h3>"
            });
        clearInterval(timer)
    }
}, 100)