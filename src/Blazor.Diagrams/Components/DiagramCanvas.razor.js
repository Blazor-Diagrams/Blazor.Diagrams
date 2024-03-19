let canvases = {};
let isObservingResize = false;

const mo = new MutationObserver(() => {
    for (id in canvases) {
        const canvas = canvases[id];
        const lastBounds = canvas.lastBounds;
        const bounds = canvas.elem.getBoundingClientRect();
        if (lastBounds.left !== bounds.left ||
            lastBounds.top !== bounds.top ||
            lastBounds.width !== bounds.width ||
            lastBounds.height !== bounds.height) {
            updateBounds(canvas);
        }
    }
})

const ro = new ResizeObserver(entries => {
    for (const entry of entries) {
        let id = Array.from(entry.target.attributes).find(e => e.name.startsWith('_bl')).name.substring(4);
        let element = canvases[id];
        if (element) {
            updateBounds(element);
        }
    }
})

export function getBoundingClientRect(el) {
    return el.getBoundingClientRect();
}

function updateBounds(canvas) {

    canvas.lastBounds = canvas.elem.getBoundingClientRect();
    canvas.ref.invokeMethodAsync('OnResize', canvas.lastBounds)
}

export function observe(element, ref, id) {
    if (isObservingResize === false) {
        mo.observe(document.body, { childList: true, subtree: true });
        window.addEventListener('scroll', () => {
            for (id in canvases) {
                const canvas = canvases[id];
                updateBounds(canvas)
            }
        })

        isObservingResize = true;
    }

    if (!element) return;
    ro.observe(element);
    if (element.classList.contains("diagram-canvas")) {
        canvases[id] = {
            elem: element,
            ref: ref,
            lastBounds: element.getBoundingClientRect()
        };
    }
}

export function unobserve(element, id) {
    if (element) {
        ro.unobserve(element);
    }
    delete canvases[id];
}