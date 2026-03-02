var zBlazorDiagramsInternalObject = {
    canvases: {},
    tracked: {},
    getBoundingClientRect: el => {
        return el.getBoundingClientRect();
    },
    mo: new MutationObserver(() => {
        for (id in zBlazorDiagramsInternalObject.canvases) {
            const canvas = zBlazorDiagramsInternalObject.canvases[id];
            const lastBounds = canvas.lastBounds;
            const bounds = canvas.elem.getBoundingClientRect();
            if (lastBounds.left !== bounds.left || lastBounds.top !== bounds.top || lastBounds.width !== bounds.width ||
                lastBounds.height !== bounds.height) {
                canvas.lastBounds = bounds;
                canvas.ref.invokeMethodAsync('OnResize', bounds);
            }
        }
    }),
    ro: new ResizeObserver(entries => {
        for (const entry of entries) {
            let id = Array.from(entry.target.attributes).find(e => e.name.startsWith('_bl')).name.substring(4);
            let element = zBlazorDiagramsInternalObject.tracked[id];
            if (element) {
                element.ref.invokeMethodAsync('OnResize', entry.target.getBoundingClientRect());
            }
        }
    }),
    observe: (element, ref, id) => {
        if (!element) return;
        zBlazorDiagramsInternalObject.ro.observe(element);
        zBlazorDiagramsInternalObject.tracked[id] = {
            ref: ref
        };
        if (element.classList.contains("diagram-canvas")) {
            zBlazorDiagramsInternalObject.canvases[id] = {
                elem: element,
                ref: ref,
                lastBounds: element.getBoundingClientRect()
            };
        }
    },
    unobserve: (element, id) => {
        if (element) {
            zBlazorDiagramsInternalObject.ro.unobserve(element);
        }
        delete zBlazorDiagramsInternalObject.tracked[id];
        delete zBlazorDiagramsInternalObject.canvases[id];
    }
};
window.ZBlazorDiagrams = zBlazorDiagramsInternalObject;
window.addEventListener('scroll', () => {
    for (id in zBlazorDiagramsInternalObject.canvases) {
        const canvas = zBlazorDiagramsInternalObject.canvases[id];
        canvas.lastBounds = canvas.elem.getBoundingClientRect();
        canvas.ref.invokeMethodAsync('OnResize', canvas.lastBounds);
    }
});
zBlazorDiagramsInternalObject.mo.observe(document.body, {childList: true, subtree: true});