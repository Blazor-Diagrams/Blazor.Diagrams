var s = {
    getBoundingClientRect: el => {
        return el.getBoundingClientRect();
    },
    tracked: {},
    ro: new ResizeObserver(entries => {
        for (const entry of entries) {
            let id = Array.from(entry.target.attributes).find(e => e.name.startsWith('_bl')).name.substring(4);
            let element = window.ZBlazorDiagrams.tracked[id];
            if (element) {
                element.ref.invokeMethodAsync('OnResize', {
                    'Width': entry.contentRect.width,
                    'Height': entry.contentRect.height
                });
            }
        }
    }),
    observe: (element, ref, id) => {
        s.ro.observe(element);
        s.tracked[id] = {
            ref: ref
        };
    },
    unobserve: (element, id) => {
        s.ro.unobserve(element);
        delete s.tracked[id];
    }
};
window.ZBlazorDiagrams = s;