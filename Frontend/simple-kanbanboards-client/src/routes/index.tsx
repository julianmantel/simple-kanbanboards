import { createBrowserRouter } from "react-router-dom";
import AuthPage from "../auth/pages/AuthPage";
import GuestRoute from "../auth/components/GuestRoute";
import ProtectedRoute from "../auth/components/ProtectedRoute";
import AppLayout from "../shared/components/AppLayout";
import Home from "../projects/pages/Home";
import ProjectPage from "../projects/pages/ProjectPage";
import BoardPage from "../boards/pages/BoardPage";
import NotFound from "../shared/components/NotFound";

export const router = createBrowserRouter([
  {
    path: '/',
    element: <GuestRoute />,
    children: [
      {
        index: true,
        element: <AuthPage />,
      },
    ],
  },

  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <AppLayout />,
        children: [
          {
            path: '/home',
            element: <Home />,
          },
          {
            path: '/projects/:projectId',
            element: <ProjectPage />,
          },
          {
            path: '/projects/:projectId/boards/:boardId',
            element: <BoardPage />,
          },
        ],
      },
    ],
  },

  {
    path: '*',
    element: <NotFound />,
  },
]);
