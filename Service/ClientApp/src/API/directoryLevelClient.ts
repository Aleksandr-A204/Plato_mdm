import axios from "axios";
import type { DirectoryLevel } from "@/models/models";

class LevelClient {
  private static readonly apiUrl = "/api/mdmdirectorylevel";

  static async getAllLevels(): Promise<Array<DirectoryLevel>> {
    return await axios.get(this.apiUrl, {})
      .then(response => response.data);
  }

  // static async addDirectory(directoryParams: models.Directory): Promise<any> {
  //   return await axios.post(this.apiUrl, directoryParams)
  //     .then(response => response.data);
  // }

  // static async ubdateDirectory(directoryParams: models.Directory): Promise<any> {
  //   return await axios.put(`${this.apiUrl}/${directoryParams.id}`, directoryParams)
  //     .then(response => response.data);
  // }

  // static async deleteDirectory(id: String): Promise<any> {
  //   return await axios.delete(`${this.apiUrl}/${id}`, {})
  //     .then(response => response.data);
  // }
}

export default LevelClient;
