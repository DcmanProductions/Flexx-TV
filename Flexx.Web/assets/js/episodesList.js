var test = {};
document.querySelector('body').style.background = "black";
document.getElementById("SeasonListView").innerHTML = "<h3>Searching for TV Shows...</h3>"
var html = ""//"<ul>";
var timer = setInterval(() => {
    if (userName != "") {
        var id = window.location.hash.replace("#", "").split('-')[0].replace("-", "");
        var season = window.location.hash.replace("#", "").split('-')[1].replace("-", "");
        let url = `${server}/api/streaming/tv/${userName}/get/${id}/${season}`
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
                    let episodeGenericName = `S${season > 9 ? season : "0" + season}E${item["number"] > 9 ? item["number"] : "0" + item["number"]}`
                    html += `
        <div class="mediaItem tv ${cName}" onclick="window.location.href = '/Library/TV/Seasons/Episodes/Episode#${id}-${season}-${item["number"]}'">
        <img src="${item["posterURL"]}" />
        <div class="mediaPosterProgressIndicator" style="width: ${watchedPercentage}%;"></div>
        <div class="movieTitle">
        ${item["name"]}
            <div class="movieYear">
            ${episodeGenericName}
            </div>
        </div>
    </div>
                            `;
                })
            }).then(() => {
                // html += "</ul>";
            }).then(() => {
                document.getElementById("SeasonListView").innerHTML = html;
                document.querySelector('body').style.backgroundRepeat = "no-repeat"
                document.querySelector('body').style.backgroundSize = "cover"
                document.querySelector('body').style.backgroundAttachment = "fixed"
            })
            .catch((err) => {
                console.log(err)
                document.getElementById("SeasonListView").innerHTML = "<h3>Unable to Connect to Server!</h3>"
            });
        clearInterval(timer)
    }
}, 100)