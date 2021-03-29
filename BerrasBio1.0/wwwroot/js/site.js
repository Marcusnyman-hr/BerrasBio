let navToggle = document.getElementById("nav-toggle");
let navigation = document.getElementById("navigation");
let navIsToggled = false;

let movieDetailsNav = document.getElementById("movie-details-nav-list");

let aboutSection = document.getElementById("movie-details-inner-about");
let showingsSection = document.getElementById("movie-details-inner-showings");
let trailerSection = document.getElementById("movie-details-inner-trailer");

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




//movieDetailsNav.addEventListener('click', function (event) {
//  showingsSection.classList.remove("display-none")
//  if (event.target.parentElement.getAttribute("data-link") != null) {
//    let links = document.getElementById("movie-details-nav-list").getElementsByTagName("li");
//    for (let i = 0; i < links.length; i++) {
//      links[i].classList.remove("active")
//    }
//    let link = event.target.parentElement.getAttribute("data-link");
//    switch (link) {
//      case "about":
//        event.target.parentElement.classList.add("active");
//        showingsSection.classList.add("display-none")
//        trailerSection.classList.add("display-none")
//        aboutSection.classList.remove("display-none")
//        console.log("About click");
//        break;
//      case "showings":
//        event.target.parentElement.classList.add("active");
//        aboutSection.classList.add("display-none")
//        trailerSection.classList.add("display-none")
//        showingsSection.classList.remove("display-none")
//        console.log("Showing click");
//        break;
//      case "trailer":
//        event.target.parentElement.classList.add("active");
//        showingsSection.classList.add("display-none")
//        aboutSection.classList.add("display-none")
//        trailerSection.classList.remove("display-none")
//        console.log("Trailer click");
//        break;
//    }
//  }
//}, false);

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

