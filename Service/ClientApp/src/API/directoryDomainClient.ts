import axios from "axios";
import type { DirectoryDomain } from "@/models/models";

class DomainClient {
  private static readonly apiUrl = "/api/mdmdirectorydomain";

  static async getAllDomains(): Promise<Array<DirectoryDomain>> {
    return await axios.get(this.apiUrl, {})
      .then(response => response.data);
  }
}

export default DomainClient;
