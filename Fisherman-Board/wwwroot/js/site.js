document.addEventListener("DOMContentLoaded", () => {
  document.body.classList.add("is-ready");

  const header = document.querySelector(".site-header");
  const statusBanner = document.querySelector(".status-banner");
  const revealItems = document.querySelectorAll("[data-reveal]");
  const parallaxItems = document.querySelectorAll("[data-parallax]");
  const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

  const syncHeaderState = () => {
    if (!header) {
      return;
    }

    header.classList.toggle("is-scrolled", window.scrollY > 18);
  };

  syncHeaderState();
  window.addEventListener("scroll", syncHeaderState, { passive: true });

  revealItems.forEach((item) => {
    const order = Number(item.getAttribute("data-reveal") || "0");
    item.style.setProperty("--reveal-delay", `${order * 90}ms`);
  });

  if (reduceMotion) {
    revealItems.forEach((item) => item.classList.add("is-visible"));
  } else if ("IntersectionObserver" in window) {
    const observer = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          entry.target.classList.add("is-visible");
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.2, rootMargin: "0px 0px -40px 0px" });

    revealItems.forEach((item) => observer.observe(item));
  } else {
    revealItems.forEach((item) => item.classList.add("is-visible"));
  }

  if (!reduceMotion && parallaxItems.length > 0) {
    window.addEventListener("pointermove", (event) => {
      const offsetX = (event.clientX / window.innerWidth - 0.5) * 16;
      const offsetY = (event.clientY / window.innerHeight - 0.5) * 10;

      parallaxItems.forEach((item) => {
        item.style.setProperty("--parallax-x", `${offsetX}px`);
        item.style.setProperty("--parallax-y", `${offsetY}px`);
      });
    }, { passive: true });
  }

  if (statusBanner) {
    window.setTimeout(() => {
      statusBanner.style.transition = "opacity 0.3s ease, transform 0.3s ease";
      statusBanner.style.opacity = "0";
      statusBanner.style.transform = "translateY(-6px)";
      window.setTimeout(() => statusBanner.remove(), 320);
    }, 4200);
  }
});
