
(function (a) { a(document).ready(function () { a("html").hasClass("is-builder") || (a(".mbr-gallery-item").on("click", "a", function (b) { a(this)[0].hasAttribute("data-target") && "" !== a(this).attr("data-target") && a("#" + a(this).attr("data-target").replace("#", "")).modal("show") }), setTimeout(function () { a(".mbr-popup .modal-content *").removeClass("hidden animate__animated animate__delay-1s animate__fadeInUp") }, 0)) }) })(jQuery);
