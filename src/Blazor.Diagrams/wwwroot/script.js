var s = {
    canvas: null,
    getBoundingClientRect: el => {
        return el.getBoundingClientRect();
    },
    tracked: {},
    ro: new ResizeObserver(entries => {
        for (const entry of entries) {
            let id = Array.from(entry.target.attributes).find(e => e.name.startsWith('_bl')).name.substring(4);
            let element = window.ZBlazorDiagrams.tracked[id];
            if (element) {
                element.ref.invokeMethodAsync('OnResize', entry.target.getBoundingClientRect());
            }
        }
    }),
    observe: (element, ref, id, isCanvas) => {
        s.ro.observe(element);
        s.tracked[id] = {
            ref: ref
        };
        if (isCanvas) {
            s.canvas = {
                elem: element,
                ref: ref
            }
        }
    },
    unobserve: (element, id) => {
        s.ro.unobserve(element);
        delete s.tracked[id];
    },
    
};
window.ZBlazorDiagrams = s;
window.addEventListener('scroll', () => {
    s.canvas.ref.invokeMethodAsync('OnResize', s.canvas.elem.getBoundingClientRect());
});