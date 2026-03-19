document.addEventListener("DOMContentLoaded", () => {
  const statusBanner = document.querySelector(".status-banner");

  if (!statusBanner) {
    return;
  }

  window.setTimeout(() => {
    statusBanner.style.transition = "opacity 0.3s ease, transform 0.3s ease";
    statusBanner.style.opacity = "0";
    statusBanner.style.transform = "translateY(-6px)";
    window.setTimeout(() => statusBanner.remove(), 320);
  }, 4200);
});
