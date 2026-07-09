export default function LeftPanel() {
  return (
    <div className="p-10 flex flex-col justify-between relative overflow-hidden bg-gradient-to-br from-[#085041] via-[#0F6E56] to-[#1D9E75]">
      <div className="z-10 flex items-center gap-2.5">
        <div className="w-15 h-15 bg-white/15 rounded-lg flex items-center justify-center">
          <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="white" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <rect x="3" y="3" width="5" height="18" rx="1" />
            <rect x="9.5" y="7" width="5" height="14" rx="1" />
            <rect x="16" y="3" width="5" height="18" rx="1" />
          </svg>
        </div>
        <span className="text-white font-medium tracking-wide text-xl">Simple Kanban Boards</span>
      </div>

      <div className="z-10 mt-auto mb-auto space-y-6">
        <h2 className="text-white text-4xl font-medium leading-relaxed text-center">
          Organiza tu proyecto!
        </h2>

        <div className="space-y-3.5 mt-8">
          <div className="flex items-center gap-3 mb-7">
            <div className="w-12 h-12 bg-white/10 rounded-lg flex items-center justify-center shrink-0 me-4">
              <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="white" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <rect x="3" y="3" width="7" height="7" rx="1" />
                <rect x="14" y="3" width="7" height="7" rx="1" />
                <rect x="3" y="14" width="7" height="7" rx="1" />
                <rect x="14" y="14" width="7" height="7" rx="1" />
              </svg>
            </div>
            <div>
              <p className="text-white text-xl font-medium">Tableros visuales</p>
              <p className="text-white/60 text-lg">Creá columnas personalizadas para cada etapa de tu flujo</p>
            </div>
          </div>
          <div className="flex items-center gap-3 mb-7">
            <div className="w-12 h-12 bg-white/10 rounded-lg flex items-center justify-center shrink-0 me-4">
              <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="white" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2" />
                <circle cx="9" cy="7" r="4" />
                <path d="M23 21v-2a4 4 0 00-3-3.87" />
                <path d="M16 3.13a4 4 0 010 7.75" />
              </svg>
            </div>
            <div>
              <p className="text-white text-xl font-medium">Colaboración en equipo</p>
              <p className="text-white/60 text-lg">Trabajá junto a tu equipo con actualizaciones en tiempo real</p>
            </div>
          </div>
          <div className="flex items-center gap-3">
            <div className="w-12 h-12 bg-white/10 rounded-lg flex items-center justify-center shrink-0 me-4">
              <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="white" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <path d="M7 7l9.2 9.2M17 7v10H7" />
              </svg>
            </div>
            <div>
              <p className="text-white text-xl font-medium">Arrastrá y soltá</p>
              <p className="text-white/60 text-lg">Mové tarjetas entre columnas con un simple gesto</p>
            </div>
          </div>
        </div>
      </div>

      <div className="pointer-events-none absolute -top-15 -right-15 w-65 h-65 rounded-full border border-white/8" />
      <div className="pointer-events-none absolute -left-10 bottom-10 w-45 h-45 rounded-full border border-white/6" />
    </div>
  );
}
