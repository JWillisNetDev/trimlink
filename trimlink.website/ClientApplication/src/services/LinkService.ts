import axios from 'axios'

export interface LinkCreate {
  redirectToUrl: string,
  duration: string | null
}

export default class LinkService {
  
  public static async createLink(redirectToUrl: string, duration?: string): Promise<string | undefined> {
    const linkCreate: LinkCreate = {
      redirectToUrl,
      duration: duration ?? null
    }
    const link = await axios
        .post('/api/Links', linkCreate)
        .then(response => response.data)
        .catch(error => {
          if (error.response) {
            const {data, status, headers} = error.response
            console.log(data)
            console.log(status)
            console.log(headers)
          }
          return undefined
        })
    return link
  }

  public static async getLinkByToken(token: string): Promise<string | undefined> {
    const link = await axios
      .get(`/api/links/${token}`)
      .then((response) => response.data)
      .catch((error) => {
        console.log(error);
        return undefined
      })
    return link
  }
}
