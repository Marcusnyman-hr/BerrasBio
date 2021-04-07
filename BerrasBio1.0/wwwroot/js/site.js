let navToggle = document.getElementById("nav-toggle");
let navigation = document.getElementById("navigation");
let navIsToggled = false;

//let movieDetailsNav = document.getElementById("movie-details-nav-list");

//let aboutSection = document.getElementById("movie-details-inner-about");
//let showingsSection = document.getElementById("movie-details-inner-showings");
//let trailerSection = document.getElementById("movie-details-inner-trailer");

navToggle.addEventListener("click", () => {
  if (!navIsToggled) {
    navToggle.classList.add("is-active");
    navigation.classList.add("nav-active");
  } else {
    navToggle.classList.remove("is-active");
    navigation.classList.remove("nav-active");
  }
  navIsToggled = !navIsToggled;
});



$(document).ready(function () {
  $('.main-carousel').slick({
    arrows: false,
    autoplay: true,
    autoplaySpeed: 2500,
    speed: 1000,
    fade: true,
    cssEase: 'linear'
  });
  $('.sub-carousel').slick({
    infinite: true,
    slidesToShow: 6,
    slidesToScroll: 3,
    arrows: false,
    dots: true,
  });
});

