import axios from 'axios'

export interface LinkCreate {
    redirectToUrl: string
    isNeverExpires: boolean
    duration: string
}

export default class LinkService {
    private static async baseCreateLink(redirectToUrl: string, duration: string, isNeverExpires: boolean) {
        const linkCreate: LinkCreate = {
            redirectToUrl,
            duration,
            isNeverExpires
        }
        const link = await axios.post('/api/Links', linkCreate)
            .then(response => response.data)
            .catch(error => undefined)
        return link
    }
    
    public static async createLink(redirectToUrl: string): Promise<string|undefined> {
        return await LinkService.baseCreateLink(redirectToUrl, "0.00:00:00", true)
    }
    
    public static async createLinkExpires(redirectToUrl: string, duration: string): Promise<string|undefined> {
        return await LinkService.baseCreateLink(redirectToUrl, duration, false)
    }
    
    public static async  getLinkByToken(token: string): Promise<string|undefined> {
        const link = await axios.get(`/api/links/${token}`)
            .then(response => response.data)
            .catch(error => undefined)
        return link
    }
}