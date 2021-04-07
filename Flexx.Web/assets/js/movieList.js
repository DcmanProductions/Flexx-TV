var test = {};
document.querySelector('body').style.background = "black";
document.getElementById("mediaView").innerHTML = "<h3>Searching for Movies...</h3>"
var html = "<ul>";
fetch(`${server}/api/streaming/movies/${userName}/get/`)
    .then(response => response.json())
    .then((data) => {
        test = data;
    }).then(() => {
        test["items"].forEach(item => {
            let watched = false;
            watched = item["watched"];
            watchedPercentage = item["watchedPercentage"];
            let cName = (watched || watchedPercentage > 0) ? "" :  "unwatched"
            html += `
                    <li>
                    <div class="mediaItem ${cName}" style="position: relative" data-movie-name="" data-movie-percentage=0 id="" data-movie-watched=false  onmouseover="document.querySelector('body').style.background = 'url(${item["coverURL"]})'" onclick="window.location.href = '/Library/Movies/View/#${item["id"]}'">
                    <div class="mediaPosterProgressIndicator" style="width: ${watchedPercentage}%;"></div>
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
    }).then(() => console.log("First Fetch - Should Return Last")).then(() => {
        html += "</ul>";
    }).then(() => {
        document.getElementById("mediaView").innerHTML = html;
        document.querySelector('body').style.background = `url(${test["items"][0]["coverURL"]})`;
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