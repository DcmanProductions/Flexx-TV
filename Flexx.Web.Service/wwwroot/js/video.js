var timer;
var isFullscreen = false;
var overlay = document.querySelector('#videoOverlay')
var player = document.querySelector('#video')

var currentTime = document.querySelector('#currentTime')
var totalTime = document.querySelector('#totalTime')
var endTime = document.querySelector('#endTime')
var videoProgressBar = document.querySelector('#video-progress-completed')
var volumeProgressBar = document.querySelector('#volume-progress-completed')
var volumeBar = document.querySelector('#volume')
var ProgressBar = document.querySelector('#progress')
var fullscreenButton = document.querySelector('#fullscreenBtn')
var backwardButton = document.querySelector('#backward')

var stepBackward = document.querySelector('#skipBackward')
var stepForward = document.querySelector('#skipForward')

var video = document.querySelector('video');
var playButtons = document.querySelectorAll(".playBtn");
function init() {
    updateTimer();
    togglePlaying(true);
}
video.addEventListener("timeupdate", () => updateTimer());
function updateTimer() {
    var percent = ((video.currentTime / video.duration) * 100) + "%";
    var totalHours = Math.floor(video.duration / 60 / 60);
    var totalMinutes = Math.floor(video.duration / 60) - (totalHours * 60);
    var totalSeconds = Math.floor(video.duration % 60);

    var currentHours = Math.floor(video.currentTime / 60 / 60);
    var currentMinutes = Math.floor(video.currentTime / 60) - (currentHours * 60);
    var currentSeconds = Math.floor(video.currentTime % 60);

    currentHours = (currentHours > 9 ? currentHours : "0" + currentHours);
    currentMinutes = currentMinutes > 9 ? currentMinutes : "0" + currentMinutes;
    currentSeconds = currentSeconds > 9 ? currentSeconds : "0" + currentSeconds;

    totalHours = (totalHours > 9 ? totalHours : "0" + totalHours);
    totalMinutes = totalMinutes > 9 ? totalMinutes : "0" + totalMinutes;
    totalSeconds = totalSeconds > 9 ? totalSeconds : "0" + totalSeconds;

    totalTime.innerHTML = `${totalHours}:${totalMinutes}:${totalSeconds}`;
    currentTime.innerHTML = `${currentHours}:${currentMinutes}:${currentSeconds}`;

    var date = new Date(new Date().getTime() + (video.duration - video.currentTime) * 1000);
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'P.M.' : 'A.M.';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    endTime.innerHTML = `${hours}:${minutes} ${ampm}`;

    videoProgressBar.style.width = percent;

    if (video.ended) {
        showOverlay();
        video.pause();
    }
    volumeProgressBar.style.width = video.volume * 100 + "%";
}

ProgressBar.addEventListener('click', e => {
    var pos = (e.pageX - (ProgressBar.offsetLeft + ProgressBar.offsetParent.offsetLeft)) / ProgressBar.offsetWidth;
    video.currentTime = pos * video.duration;
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
});
volumeBar.addEventListener('click', e => {
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
});
volumeBar.addEventListener('click', e => {
    var pos = (e.pageX - (volumeBar.offsetLeft + volumeBar.offsetParent.offsetLeft)) / volumeBar.offsetWidth;
    video.volume = pos;
    updateTimer();
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
});
fullscreenButton.addEventListener('click', e => {
    toggleFullscreen();
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
});
backward.addEventListener('click', e => {
    video.currentTime = 0;
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
});
stepForward.addEventListener('click', e => {
    video.currentTime += 30;
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
});
stepBackward.addEventListener('click', e => {
    video.currentTime -= 30;
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
});

overlay.addEventListener("mouseout", () => {
    if (!video.paused) {
        timer = setTimeout(() => {
            hideOverlay();
        }, 1000);
    }
});
overlay.addEventListener("mouseover", () => {
    showOverlay();
    clearTimeout(timer);
    if (!video.paused) {
        timer = setTimeout(() => {
            hideOverlay();
        }, 1000);
    }
});
overlay.addEventListener("mousemove", () => {
    showOverlay();
    clearTimeout(timer);
    if (!video.paused) {
        timer = setTimeout(() => {
            hideOverlay();
        }, 1000);
    }
});
document.querySelector('#overlayBG').addEventListener("click", () => {
    togglePlaying();
});
playButtons.forEach(e => e.addEventListener("click", () => {
    togglePlaying();
}));
document.querySelector('#overlayBG').addEventListener("dblclick", () => {
    toggleFullscreen();
})

function toggleFullscreen() {
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
    if (isFullscreen)
        closeFullscreen()
    else
        openFullscreen()
}

function openFullscreen() {
    isFullscreen = true;
    fullscreenButton.children[0].src = "/images/svg/compress-solid.svg";
    if (player.requestFullscreen) {
        player.requestFullscreen();
    } else if (player.webkitRequestFullscreen) { /* Safari */
        player.webkitRequestFullscreen();
    } else if (player.msRequestFullscreen) { /* IE11 */
        player.msRequestFullscreen();
    }
}

function closeFullscreen() {
    isFullscreen = false;
    fullscreenButton.children[0].src = "/images/svg/expand-solid.svg";
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.webkitExitFullscreen) { /* Safari */
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) { /* IE11 */
        document.msExitFullscreen();
    }
}

function togglePlaying(shouldPlay) {
    showOverlay();
    if (shouldPlay === true) {
        play();
    } else if (shouldPlay === false) {
        play();
        setTimeout(()=>{
            pause();
        }, 100);
    } else {
        if (video.paused) {
            play();
        } else {
            pause();
        }
    }
}

function play() {
    video.play();
    playButtons.forEach(e => e.src = "/images/svg/pause-solid.svg");
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
}
function pause() {
    video.pause();
    playButtons.forEach(e => e.src = "/images/svg/play-solid.svg");
    clearTimeout(timer);
}

function hideOverlay() {
    overlay.classList.remove('overlayActive');
    overlay.style.cursor = "none";
}
function showOverlay() {
    overlay.classList.add('overlayActive');
    overlay.style.cursor = "default";
}

init();