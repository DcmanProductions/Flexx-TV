<?php
    $trailer = $_GET("trailer");
    $id = $_GET("id");
?>

<link rel="stylesheet" href="/assets/css/main.css">
<link rel="stylesheet" href="/assets/css/video.css">
<div id="video">
    <div id="videoOverlay" class="overlayActive">
        <div id="overlayBG"></div>
        <div id="nonActiveArea"></div>
        <div id="videoTitle">
            <div id="videoYear"></div>
        </div>
        <div id="closeBtn">
            <img src="/assets/images/svg/Close.svg" alt="">
        </div>
        <div id="Navigation">
            <div id="progress">
                <div id="video-progress-completed"></div>
            </div>

            <div id="settings">
                <img src="/assets/images/svg/sliders-h-solid.svg" alt="">
            </div>
            <div id="controls">
                <ul>
                    <li style="display:none;">
                        <div id="backward">
                            <img src="/assets/images/svg/fast-backward-solid.svg" alt="">
                        </div>
                    </li>
                    <li>
                        <div id="skipBackward">
                            <img src="/assets/images/svg/step-backward-solid.svg" alt="">
                        </div>
                    </li>
                    <li>
                        <div id="play">
                            <img class="playBtn" src="/assets/images/svg/play-solid.svg" alt="">
                        </div>
                    </li>
                    <li>
                        <div id="skipForward">
                            <img src="/assets/images/svg/step-forward-solid.svg" alt="">
                        </div>
                    </li>
                    <li style="display:none;">
                        <div id="forward">
                            <img src="/assets/images/svg/fast-forward-solid.svg" alt="">
                        </div>
                    </li>
                    <li id="volumeListItem">
                        <div id="volume">
                            <div id="volume-progress-completed"></div>
                        </div>
                    </li>
                </ul>
            </div>
            <div id="timestamp">
                <div id="currentTime">00:00</div>
                <div id="timeStampSeperator"></div>
                <div id="totalTime">00:00</div>
                <div id="timeStampSeperator"></div>
                <div id="endTime">00:00</div>
            </div>
            <div id="fullscreenBtn">
                <img src="/assets/images/svg/expand-solid.svg" alt="">
            </div>
        </div>
    </div>
    <video src="" autoplay></video>
</div>
<div id="backgroundTasks" style="display:none"></div>

<!-- Firebase -->
<script src="https://www.gstatic.com/firebasejs/8.3.2/firebase-app.js"></script>
<script src="https://www.gstatic.com/firebasejs/8.3.2/firebase-firestore.js"></script>
<script src="https://www.gstatic.com/firebasejs/8.3.2/firebase-analytics.js"></script>
<script src="https://www.gstatic.com/firebasejs/8.3.2/firebase-auth.js"></script>

<script src="/assets/js/user.js"></script>
<script src="/assets/js/auth.js"></script>

<script src="/assets/js/video.js"></script>
<?php
if($trailer == "true"){
    echo '<script src="/assets/js/trailer.js"></script>';
}else{
    echo '<script src="/assets/js/watch.js"></script>';
}
?>