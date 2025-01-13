import axios from "axios";
import type { Directory } from "@/models/models";

class DirectoryClient {
  private static readonly apiUrl = "/api/mdmdirectory";

  static async getDirectoryById(id: string | string[]): Promise<Directory> {
    return await axios.get(`${this.apiUrl}/${id}`, {})
      .then(response => response.data);
  }

  static async getDirectories(): Promise<Array<Directory>> {
    return await axios.get(`${this.apiUrl}`, {})
      .then(response => response.data);
  }

  static async addDirectory(directoryParams: Directory): Promise<any> {
    return await axios.post(this.apiUrl, directoryParams)
      .then(response => response.data);
  }

  static async ubdateDirectory(directoryParams: Directory): Promise<any> {
    return await axios.put(this.apiUrl, directoryParams)
      .then(response => response.data);
  }

  static async deleteDirectory(id: string): Promise<any> {
    return await axios.delete(`${this.apiUrl}/${id}`, {})
      .then(response => response.data);
  }

  // static async getDirectories(searchKeyWord): Promise<any> {
  //   return await axios.delete(`${this.apiUrl}/SearchKeyWord/${searchKeyWord}`, {})
  //     .then(response => response.data);
  // }
}

export default DirectoryClient;
