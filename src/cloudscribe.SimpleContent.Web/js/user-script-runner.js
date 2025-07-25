// Not currently in production - jk

document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("user-script-container");
    if (!container) return;

    let scriptContent = container.getAttribute("data-user-script");
    if (!scriptContent) return;
    
    try {
        scriptContent = JSON.parse(`"${scriptContent}"`);

        const scriptEl = document.createElement("script");  // may violate CSP 'UnsafeInline'
        scriptEl.type = "text/javascript";
        scriptEl.textContent = scriptContent;

        document.body.appendChild(scriptEl);
    } catch (error) {
        console.error("Error executing user script:", error);
    }
});