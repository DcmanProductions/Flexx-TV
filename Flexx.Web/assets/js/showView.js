import("/assets/js/user.js")
var timer = setInterval(() => {
    if (userName != "") {
        if (window.location.hash) {
            var id = window.location.hash.replace("#", "");
            var movie = {};
            fetch(`${server}/api/streaming/tv/${userName}/get/${id}`)
                .then(response => response.json())
                .then((data) => {
                    movie = data["items"];
                }).then(() => {
                    // Poster Image
                    document.getElementById("moviePoster").style.background = `url(${movie["posterURL"]})`;
                    document.getElementById("moviePoster").style.backgroundRepeat = "no-repeat";
                    document.getElementById("moviePoster").style.backgroundSize = "cover";

                    // Background Image
                    document.querySelector("body").style.background = `url(${movie["coverURL"]})`;
                    document.querySelector("body").style.backgroundRepeat = "no-repeat";
                    document.querySelector("body").style.backgroundSize = "cover";
                    document.querySelector("body").style.backgroundAttachment = "fixed";

                    document.querySelector('title').text = `${movie["name"]} - Flexx TV`;

                    document.getElementById("movieTitle").innerHTML = movie["name"];
                    document.getElementById("movieSummery").innerHTML = movie["summery"];
                    document.getElementById("language").innerHTML = movie["language"];

                    var exe = "";
                    for (let i = 0; i < Array.from(movie["writers"]).length; i++) {
                        if (i >= 3) break;
                        let item = movie["writers"][i]
                        exe += `
                        <li>
                                        <div class="executiveTitle">Writer</div>
                                        <div class="executiveName">${item}</div>
                                    </li>
                        `;
                    }

                    document.getElementById("executiveList").innerHTML = exe;


                    exe = "";
                    for (let i = 0; i < Array.from(movie["genres"]).length; i++) {
                        if (i >= 3) break;
                        let item = movie["genres"][i]
                        if (i == 2)
                            exe += `<li>${item}</li>`;
                        else
                            exe += `<li>${item},</li>`;
                    }
                    document.getElementById("genresList").innerHTML = exe;

                    exe = "";
                    for (let i = 0; i < Array.from(movie["actors"]).length; i++) {
                        if (i >= 4) break;
                        let item = movie["actors"][i]
                        exe += `
                        <li>
                            <div class="castCard">
                                <img class="castImage" src="${item["profileURL"]}" />
                                <div class="actorName">${item["name"]}</div>
                                <div class="actorRole">${item["character"]}</div>
                            </div>
                        </li>`;
                    }
                    document.getElementById("castList").innerHTML = exe;

                }).then(() => {
                    document.getElementById("playBtn").addEventListener("click", () => {
                        window.location.href = `/Library/Movies/Watch/#${id}`;
                    });
                    document.getElementById("secPlayBtn").addEventListener("click", () => {
                        window.location.href = `/Library/Movies/Watch/#${id}`;
                    });
                })
                .catch((err) => {
                    console.log(err);
                    // window.location.href = "/Library/Movies/";
                });
        } else {
            // window.location.href = "/Library/Movies/";
        }

        $("#view").load("/Pages/TV/Seasons/list.html")

        clearInterval(timer);
    }
}, 1000)