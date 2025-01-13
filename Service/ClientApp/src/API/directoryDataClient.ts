import axios, { type AxiosRequestConfig } from "axios";
import type { DeleteDataRequest, MdmTablesData } from "@/models/models";

class DirectoryDataClient {
  private static readonly apiUrl = "/api/directorydata";

  static async getData(id: string | string[]): Promise<any> {
    return await axios.get(`${this.apiUrl}/${id}`, {})
      .then(response => response)
      .catch(exception => this.handleError(exception.response));
  }

  static async getRelatedData(tablename: string): Promise<MdmTablesData> {
    return await axios.get(`api/relateddata/${tablename}`, {})
      .then(response => response.data)
      .catch(exception => this.handleError(exception.response));
  }

  static async createRecord(tablename: string): Promise<any> {
    return await axios.post(`${this.apiUrl}/${tablename}`)
      .then(response => response)
      .catch(exception => this.handleError(exception.response));
  }

  static async updateRecords(data: any): Promise<any> {
    return await axios.put(this.apiUrl, data)
      .then(response => response)
      .catch(exception => this.handleError(exception.response));
  }

  static async deleteRecord(deleteData: DeleteDataRequest): Promise<any> {
    const config: AxiosRequestConfig = {
      headers: { "Content-Type": "application/json" },
      data: deleteData
    };
    return await axios.delete(`${this.apiUrl}`, config)
      .then(response => response)
      .catch(exception => this.handleError(exception.response));
  }

  private static handleError(error: any): any {
    if (axios.isAxiosError(error)) {
      return error.response?.data || "Произошла ошибка при выполнении запроса.";
    }
    return "Неизвестная ошибка.";
  }
}

export default DirectoryDataClient;
