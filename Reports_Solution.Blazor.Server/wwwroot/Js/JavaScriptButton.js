function autoSubmitReport() {
  const waitForParamsPanel = setInterval(() => {
    const panel = document.querySelector(".dxbrv-gridlayout-side-bar-content");

    if (panel) {
      console.log("✅ Panel de parámetros detectado");

      clearInterval(waitForParamsPanel);

      const waitForButton = setInterval(() => {
        const submitButton = Array.from(
          panel.querySelectorAll("button.dxbl-btn.dxbl-btn-primary.dxbl-btn-standalone")
        ).find(btn => btn.innerText.trim().toLowerCase() === "submit");

        if (submitButton) {
          console.log("✔ Botón Submit encontrado, haciendo click...");
          submitButton.click();
          clearInterval(waitForButton);
        } else {
          console.log("⌛ Esperando botón Submit dentro del panel...");
        }
      }, 500);
    } else {
      console.log("⏳ Esperando que aparezca el panel de parámetros...");
    }
  }, 500);
}
