var timer = setInterval(()=>{
    if(userName != ""){
        Init();
        clearInterval(timer)
    }
}, 1000);


function Init(){
    if (!window.location.hash) window.location.href = "/Library/Movies/";
    const id = window.location.hash.replace("#", "");
    const streamingAPI = `${server}/api/streaming/movies/${userName}/${id}`
    const returnAddress = `/Library/Movies/View/#${id}`;
    
    video.src = `${streamingAPI}/video`;
    
    setInterval(() => {
        if (Math.floor((video.currentTime / video.duration) * 100) > 90) {
            $("#backgroundTasks").load(`${streamingAPI}/save/watched/true`)
            $("#backgroundTasks").load(`${streamingAPI}/save/duration/0`)
        } else if (Math.floor((video.currentTime / video.duration) * 100) > 2) {
            $("#backgroundTasks").load(`${streamingAPI}/save/duration/${Math.floor(video.currentTime)}`)
        }
    }, 1000)
    
    video.addEventListener("ended", () => { window.location.href = returnAddress; });
    document.getElementById("closeBtn").addEventListener("click", () => { window.location.href = returnAddress; });
    
    function ContinueWatching(duration) {
        video.currentTime = duration;
    }
    
    var movie = {};
    fetch(`${server}/api/streaming/movies/${userName}/get/${id}`)
        .then(response => response.json())
        .then((data) => {
            movie = data["items"];
        }).then(() => {
            document.getElementById("videoTitle").innerHTML = movie["name"] + document.getElementById("videoTitle").innerHTML;
            document.getElementById("videoYear").innerHTML = movie["year"];
            ContinueWatching(movie["watchedDuration"]);
        });
}