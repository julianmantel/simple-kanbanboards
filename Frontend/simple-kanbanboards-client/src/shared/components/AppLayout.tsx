import { Outlet } from "react-router-dom";
import NavBar from "./NavBar";

export default function AppLayout() {
  return (
    <div className="min-h-screen bg-bg-dark">
      <NavBar />
      <main className="min-h-screen pt-16 p-6 lg:p-8">
        <Outlet />
      </main>
    </div>
  );
}
