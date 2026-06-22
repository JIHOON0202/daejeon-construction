/* ==========================================================
   대전시공사 - 메인 스크립트
   - 롤링배너 자동 슬라이드
   - 모바일 메뉴 토글
========================================================== */

(function () {
  "use strict";

  /* ---- 롤링배너 ---- */
  const slides    = [...document.querySelectorAll(".slide")];
  const dotsWrap  = document.querySelector(".dots");
  const prevBtn   = document.querySelector(".prev");
  const nextBtn   = document.querySelector(".next");

  if (slides.length === 0) return; // 배너가 없으면 중단

  let current = 0;
  let timer;

  function renderDots() {
    slides.forEach(function (_, index) {
      const dot = document.createElement("button");
      dot.type = "button";
      dot.setAttribute("aria-label", (index + 1) + "번째 슬라이드");
      dot.addEventListener("click", function () { showSlide(index); });
      dotsWrap.appendChild(dot);
    });
  }

  function showSlide(index) {
    current = (index + slides.length) % slides.length;
    slides.forEach(function (slide, i) {
      slide.classList.toggle("active", i === current);
    });
    [...dotsWrap.children].forEach(function (dot, i) {
      dot.classList.toggle("active", i === current);
    });
    restartTimer();
  }

  function restartTimer() {
    clearInterval(timer);
    timer = setInterval(function () { showSlide(current + 1); }, 4500);
  }

  renderDots();
  showSlide(0);

  if (nextBtn) nextBtn.addEventListener("click", function () { showSlide(current + 1); });
  if (prevBtn) prevBtn.addEventListener("click", function () { showSlide(current - 1); });

  /* ---- 모바일 메뉴 토글 ---- */
  const menuBtn = document.querySelector(".menu-btn");
  const menu    = document.querySelector(".menu");

  if (menuBtn && menu) {
    menuBtn.addEventListener("click", function () {
      menu.classList.toggle("open");
    });
    menu.querySelectorAll("a").forEach(function (link) {
      link.addEventListener("click", function () {
        menu.classList.remove("open");
      });
    });
  }

})();
