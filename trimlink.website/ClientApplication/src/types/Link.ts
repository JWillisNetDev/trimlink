export interface Link {
    id: number,
    utcDateCreated: string,
    utcDateExpires: string,
    isNeverExpires: boolean,
    token: string,
    redirectToUrl: string,
}