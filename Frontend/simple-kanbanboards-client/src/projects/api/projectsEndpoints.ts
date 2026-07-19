export const PROJECT_ENDPOINTS = {
    get_projects: '/projects/',
    get_project_by_id: (id: number) => `/projects/${id}/`,
    create_project: '/projects/',
    add_dev_to_project: '/projects/add-dev/',
    delete_project: (id: number) => `/projects/${id}/`,
    update_project: '/projects/'
}