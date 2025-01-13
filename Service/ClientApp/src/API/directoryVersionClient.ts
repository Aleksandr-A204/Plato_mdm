import axios from "axios";
import type { DirectoryVersion } from "@/models/models";

class DirectoryVersionClient {
  private static readonly apiUrl = "/api/mdmdirectoryversion";

  static async getAllVersions(): Promise<DirectoryVersion[]> {
    return await axios.get(this.apiUrl)
      .then(response => response.data);
  }

  static async getAllVersionsByDirectory(id: string | string[]): Promise<DirectoryVersion[]> {
    return await axios.get(`${this.apiUrl}/${id}`)
      .then(response => response.data);
  }

  static async addVersion(versionParams: DirectoryVersion): Promise<any> {
    return await axios.post(this.apiUrl, versionParams)
      .then(response => response.data);
  }

  static async ubdateVersion(versionParams: DirectoryVersion): Promise<any> {
    return await axios.put(this.apiUrl, versionParams)
      .then(response => response.data);
  }

  static async deleteVersion(id: string): Promise<any> {
    return await axios.delete(`${this.apiUrl}/${id}`, {})
      .then(response => response.data);
  }
}

export default DirectoryVersionClient;
