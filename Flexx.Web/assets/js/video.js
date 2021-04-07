import("/assets/web/assets/jquery/jquery.min.js")

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

var forwardButton = document.querySelector('#forward').parentElement
var backwardButton = document.querySelector('#backward').parentElement

var stepBackward = document.querySelector('#skipBackward').parentElement
var stepForward = document.querySelector('#skipForward').parentElement

var video = document.querySelector('video');
var playButton = document.querySelector(".playBtn")

var nonactivearea = document.querySelector('#nonActiveArea')

var draggingTrack = false;
var draggingVolume = false;

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
//VIDEO
ProgressBar.addEventListener('mousedown', e => {
    draggingTrack = true;
    videoProgressBar.style.transition = "0s";
    ProgressBar.style.transition = "0s";
});

//VOLUME
volumeBar.addEventListener('mousedown', e => {
    draggingVolume = true;
    volumeBar.style.transition = "0s";
    volumeProgressBar.style.transition = "0s";
});

overlay.addEventListener('mouseup', e => {
    //VIDEO
    if (draggingTrack) {
        draggingTrack = false;
        var pos = (e.pageX - (ProgressBar.offsetLeft + ProgressBar.offsetParent.offsetLeft)) / ProgressBar.offsetWidth;
        if (pos > 1) pos = 1;
        if (pos < 0) pos = 0;
        video.currentTime = pos * video.duration;
        clearTimeout(timer);
        videoProgressBar.style.transition = "";
        ProgressBar.style.transition = "";
    }
    //VOLUME
    if (draggingVolume) {
        draggingVolume = false;
        var pos = (e.pageX - (volumeBar.offsetLeft + volumeBar.offsetParent.offsetLeft)) / volumeBar.offsetWidth;
        if (pos > 1) pos = 1;
        if (pos < 0) pos = 0;
        video.volume = pos;
        clearTimeout(timer);
        volumeProgressBar.style.transition = "";
        volumeBar.style.transition = "";
    }
});
overlay.addEventListener('mousemove', e => {
    //VIDEO
    if (draggingTrack) {
        var pos = (e.pageX - (ProgressBar.offsetLeft + ProgressBar.offsetParent.offsetLeft)) / ProgressBar.offsetWidth;
        if (pos > 1) pos = 1;
        if (pos < 0) pos = 0;
        videoProgressBar.style.width = Math.floor(pos * 100) + "%"
        showOverlay();
    }

    //VOLUME
    if (draggingVolume) {
        var pos = (e.pageX - (volumeBar.offsetLeft + volumeBar.offsetParent.offsetLeft)) / volumeBar.offsetWidth;
        if (pos > 1) pos = 1;
        if (pos < 0) pos = 0;
        volumeProgressBar.style.width = Math.floor(pos * 100) + "%"
        showOverlay();
    }
});

ProgressBar.addEventListener('click', e => {
    var pos = (e.pageX - (ProgressBar.offsetLeft + ProgressBar.offsetParent.offsetLeft)) / ProgressBar.offsetWidth;
    video.currentTime = pos * video.duration;
});
document.addEventListener("mouseleave", e => { hideOverlay(); })

volumeBar.addEventListener('click', e => {
    var pos = (e.pageX - (volumeBar.offsetLeft + volumeBar.offsetParent.offsetLeft)) / volumeBar.offsetWidth;
    video.volume = pos;
    updateTimer();
});
fullscreenButton.addEventListener('click', e => {
    toggleFullscreen();
    clearTimeout(timer);
});
backward.addEventListener('click', e => {
    video.currentTime = 0;
    clearTimeout(timer);
});
stepForward.addEventListener('click', e => {
    video.currentTime += 30;
    clearTimeout(timer);
});
stepBackward.addEventListener('click', e => {
    video.currentTime -= 30;
    clearTimeout(timer);
});


nonactivearea.addEventListener("mouseover", () => {
    showOverlay();
    clearTimeout(timer);
    if (!video.paused) {
        timer = setTimeout(() => {
            hideOverlay();
        }, 1000);
    }
});
nonactivearea.addEventListener("mousemove", () => {
    showOverlay();
    clearTimeout(timer);
    if (!video.paused) {
        timer = setTimeout(() => {
            hideOverlay();
        }, 1000);
    }
});

document.addEventListener("mousemove", () => {
    showOverlay();
});

nonactivearea.addEventListener("mouseleave", () => {
    showOverlay();
    clearTimeout(timer);
});

nonactivearea.addEventListener("click", () => {
    togglePlaying();
});
playButton.parentElement.parentElement.addEventListener("click", () => {
    togglePlaying();
});
nonactivearea.addEventListener("dblclick", () => {
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
    fullscreenButton.children[0].src = "/assets/images/svg/compress-solid.svg";
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
    fullscreenButton.children[0].src = "/assets/images/svg/expand-solid.svg";
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
        setTimeout(() => {
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
    playButton.src = "/assets/images/svg/pause-solid.svg"
    clearTimeout(timer);
    timer = setTimeout(() => {
        hideOverlay();
    }, 1000);
}
function pause() {
    playButton.src = "/assets/images/svg/play-solid.svg"
    video.pause();
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

// loadContextMenuItems(`<div class="custom-cm__item disabled">Flexx TV</div>`);


var checkInterval = 50.0
var lastPlayPos = 0
var currentPlayPos = 0
var bufferingDetected = false

let throbber = document.createElement("div")
throbber.className = "spinner"
throbber.style.width = "64px"
throbber.style.height = "64px"
document.body.appendChild(throbber);

setInterval(checkBuffering, checkInterval)
function checkBuffering() {
    currentPlayPos = video.currentTime

    var offset = (checkInterval - 20) / 1000

    if (!bufferingDetected && currentPlayPos < (lastPlayPos + offset) && !video.paused) {
        bufferingDetected = true
        document.body.appendChild(throbber);
    }
    
    if (bufferingDetected && currentPlayPos > (lastPlayPos + offset) && !video.paused) {
        bufferingDetected = false
        document.body.removeChild(throbber)
    }
    lastPlayPos = currentPlayPos
}


document.getElementById("video").classList.add("noselect")