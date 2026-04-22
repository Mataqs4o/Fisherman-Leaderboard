document.addEventListener("DOMContentLoaded", () => {
  document.body.classList.add("is-ready");

  const header = document.querySelector(".site-header");
  const statusBanner = document.querySelector(".status-banner");
  const revealItems = document.querySelectorAll("[data-reveal]");
  const parallaxItems = document.querySelectorAll("[data-parallax]");
  const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
  const passwordInput = document.querySelector("#registerPassword");
  const confirmPasswordInput = document.querySelector("#registerConfirmPassword");
  const passwordMeter = document.querySelector("#passwordMeterToggle");
  const passwordMeterFill = document.querySelector("#passwordMeterFill");
  const passwordStrengthLabel = document.querySelector("#passwordStrengthLabel");
  const passwordStrengthHelp = document.querySelector("#passwordStrengthHelp");
  const passwordStrengthTips = document.querySelector("#passwordStrengthTips");
  const passwordHelpToggle = document.querySelector("#passwordHelpToggle");
  const generatePasswordButton = document.querySelector("#generatePasswordButton");
  const passwordGeneratorStatus = document.querySelector("#passwordGeneratorStatus");

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

  if (
    passwordInput &&
    confirmPasswordInput &&
    passwordMeter &&
    passwordMeterFill &&
    passwordStrengthLabel &&
    passwordStrengthHelp &&
    passwordStrengthTips &&
    passwordHelpToggle &&
    generatePasswordButton &&
    passwordGeneratorStatus
  ) {
    const strengthLevels = [
      { maxScore: 1, label: "Много слаба", className: "is-weak", width: "18%" },
      { maxScore: 2, label: "Слаба", className: "is-fair", width: "38%" },
      { maxScore: 3, label: "Средна", className: "is-medium", width: "58%" },
      { maxScore: 4, label: "Добра", className: "is-good", width: "78%" },
      { maxScore: 6, label: "Силна", className: "is-strong", width: "100%" }
    ];

    const getRandomIndex = (max) => {
      if (window.crypto && window.crypto.getRandomValues) {
        const values = new Uint32Array(1);
        window.crypto.getRandomValues(values);
        return values[0] % max;
      }

      return Math.floor(Math.random() * max);
    };

    const shuffle = (items) => {
      const copy = [...items];

      for (let index = copy.length - 1; index > 0; index -= 1) {
        const swapIndex = getRandomIndex(index + 1);
        [copy[index], copy[swapIndex]] = [copy[swapIndex], copy[index]];
      }

      return copy;
    };

    const generateStrongPassword = () => {
      const groups = {
        lower: "abcdefghjkmnpqrstuvwxyz",
        upper: "ABCDEFGHJKLMNPQRSTUVWXYZ",
        digits: "23456789",
        symbols: "!@#$%^&*()-_=+?"
      };

      const allChars = Object.values(groups).join("");
      const passwordChars = [
        groups.lower[getRandomIndex(groups.lower.length)],
        groups.upper[getRandomIndex(groups.upper.length)],
        groups.digits[getRandomIndex(groups.digits.length)],
        groups.symbols[getRandomIndex(groups.symbols.length)]
      ];

      while (passwordChars.length < 16) {
        passwordChars.push(allChars[getRandomIndex(allChars.length)]);
      }

      return shuffle(passwordChars).join("");
    };

    const getStrengthState = (password) => {
      let score = 0;
      const suggestions = [];

      if (password.length >= 8) {
        score += 1;
      } else {
        suggestions.push("Добави повече символи, за да стане по-дълга.");
      }

      if (password.length >= 12) {
        score += 1;
      } else {
        suggestions.push("12+ символа обикновено правят паролата по-стабилна.");
      }

      if (/[a-z]/.test(password) && /[A-Z]/.test(password)) {
        score += 1;
      } else {
        suggestions.push("Смеси главни и малки букви.");
      }

      if (/\d/.test(password)) {
        score += 1;
      } else {
        suggestions.push("Добави поне една цифра.");
      }

      if (/[^A-Za-z0-9]/.test(password)) {
        score += 1;
      } else {
        suggestions.push("Добави специален символ като !, @ или #.");
      }

      if (new Set(password).size >= 8) {
        score += 1;
      } else {
        suggestions.push("Използвай по-разнообразни символи вместо повторения.");
      }

      const level = strengthLevels.find((item) => score <= item.maxScore) || strengthLevels[strengthLevels.length - 1];

      return {
        level,
        suggestions: suggestions.slice(0, 3)
      };
    };

    const renderStrength = () => {
      const password = passwordInput.value;
      const { level, suggestions } = getStrengthState(password);

      passwordStrengthLabel.textContent = password.length === 0 ? "Няма въведена парола" : level.label;
      passwordMeter.className = `password-meter ${password.length === 0 ? "is-empty" : level.className}`;
      passwordMeterFill.style.width = password.length === 0 ? "0%" : level.width;

      passwordStrengthTips.innerHTML = "";

      const activeSuggestions = password.length === 0
        ? [
          "Започни с каквато парола искаш.",
          "Ако искаш по-сигурна, добави букви, цифри и символи.",
          "Можеш и да натиснеш бутона за генериране."
        ]
        : suggestions.length > 0
          ? suggestions
          : ["Паролата вече изглежда силна.", "Можеш да я използваш така.", "По желание я генерирай отново за друг вариант."];

      activeSuggestions.forEach((tip) => {
        const item = document.createElement("li");
        item.textContent = tip;
        passwordStrengthTips.appendChild(item);
      });
    };

    const toggleHelp = () => {
      const isHidden = passwordStrengthHelp.hasAttribute("hidden");

      if (isHidden) {
        passwordStrengthHelp.removeAttribute("hidden");
      } else {
        passwordStrengthHelp.setAttribute("hidden", "");
      }

      passwordMeter.setAttribute("aria-expanded", String(isHidden));
      passwordHelpToggle.setAttribute("aria-expanded", String(isHidden));
    };

    passwordInput.addEventListener("input", () => {
      passwordGeneratorStatus.textContent = "";
      renderStrength();
    });

    passwordHelpToggle.addEventListener("click", toggleHelp);
    passwordMeter.addEventListener("click", toggleHelp);

    generatePasswordButton.addEventListener("click", () => {
      const generatedPassword = generateStrongPassword();

      passwordInput.value = generatedPassword;
      confirmPasswordInput.value = generatedPassword;
      passwordGeneratorStatus.textContent = "Генерирана е силна парола и е попълнена и в двете полета.";

      renderStrength();
    });

    renderStrength();
  }
});
