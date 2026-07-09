import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "../../auth/context/AuthContext";
import type { Role } from "../../auth/types/user";
import { authApi } from "../../auth/api/authApi";

export default function NavBar() {
  const { user, logout, refetchUser } = useAuth();
  const navigate = useNavigate();
  const [roleDropdownOpen, setRoleDropdownOpen] = useState(false);
  const [roles, setRoles] = useState<Role[]>([]);

  useEffect(() => {
    authApi.getRoles()
      .then((loadedRoles) => {
        setRoles(Array.isArray(loadedRoles) ? loadedRoles : []);
      })
      .catch((error) => {
        console.error("Error fetching roles:", error);
      });
  }, []);

  const handleRoleSelect = async (role: Role, isActive: boolean) => {
    setRoleDropdownOpen(false);
    if (!user) return;
    if (isActive) {
      user.roles.splice(user.roles.findIndex((r) => r.id === role.id), 1);
    } else {
      user.roles.push(role);
    }

    try {
      await authApi.changeRoles(user);
      await refetchUser();
    } catch (error) {
      console.error("Error changing role:", error);
    }
  };

  const handleLogout = async () => {
    await logout();
    navigate("/");
  };

  return (
    <nav className="fixed top-0 left-0 right-0 z-40 h-16 bg-surface border-b border-border flex items-center justify-between px-6">
      {/* Left — brand + Home link */}
      <div className="flex items-center gap-6">
        <Link to="/home" className="text-base font-heading font-semibold text-text tracking-tight hover:text-teal-light transition-colors">
          Simple<span className="text-teal">KanbanBoards</span>
        </Link>
      </div>

      {/* Right — actions */}
      <div className="flex items-center gap-3">
        {/* Role switcher */}
        <div className="relative">
          <button
            onClick={() => setRoleDropdownOpen(!roleDropdownOpen)}
            className="flex items-center gap-2 px-3 py-1.5 text-sm font-medium text-muted hover:text-text hover:bg-surface2 border border-border rounded-lg transition-colors"
          >
            <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 6a3.75 3.75 0 1 1-7.5 0 3.75 3.75 0 0 1 7.5 0ZM4.501 20.118a7.5 7.5 0 0 1 14.998 0A17.933 17.933 0 0 1 12 21.75c-2.676 0-5.216-.584-7.499-1.632Z" />
            </svg>
            {user?.roles?.[0]?.name || "Roles"}
            <svg className={`size-3 transition-transform duration-200 ${roleDropdownOpen ? "rotate-180" : ""}`} fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
              <path strokeLinecap="round" strokeLinejoin="round" d="m19.5 8.25-7.5 7.5-7.5-7.5" />
            </svg>
          </button>

          {roleDropdownOpen && (
            <>
              <div className="fixed inset-0 z-10" onClick={() => setRoleDropdownOpen(false)} />
              <div className="absolute right-0 top-full mt-2 w-44 bg-surface border border-border rounded-xl shadow-xl overflow-hidden z-20">
                {roles.map((role) => {
                  const isActive = user?.roles?.some((r) => r.id === role.id) ?? false;
                  return (
                    <button
                      key={role.id}
                      onClick={() => handleRoleSelect(role, isActive)}
                      className={`flex items-center gap-2 w-full px-3 py-2 text-sm text-left transition-colors ${
                        isActive
                          ? "text-teal-light bg-teal/10"
                          : "text-muted hover:text-text hover:bg-surface2"
                      }`}
                    >
                      {isActive && (
                        <svg className="size-4 shrink-0 text-teal-light" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                          <path strokeLinecap="round" strokeLinejoin="round" d="m4.5 12.75 6 6 9-13.5" />
                        </svg>
                      )}
                      {!isActive && <span className="size-4 shrink-0" />}
                      {role.name}
                    </button>
                  );
                })}
              </div>
            </>
          )}
        </div>

        {/* Logout */}
        <button
          onClick={handleLogout}
          className="flex items-center gap-2 px-3 py-1.5 text-sm font-medium text-danger hover:bg-danger/10 border border-transparent hover:border-danger/20 rounded-lg transition-colors"
        >
          <svg className="size-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
            <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 9V5.25A2.25 2.25 0 0 0 13.5 3h-6a2.25 2.25 0 0 0-2.25 2.25v13.5A2.25 2.25 0 0 0 7.5 21h6a2.25 2.25 0 0 0 2.25-2.25V15M3 12h12m0 0-3-2.25M15 12l-3 2.25" />
          </svg>
          Logout
        </button>
      </div>
    </nav>
  );
}
