import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiBaseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUsers(): Observable<any> {
    return this.http.get(`${this.apiBaseUrl}/GetUsers`);
  }

  getUserById(id: number): Observable<any> {
    return this.http.get(`${this.apiBaseUrl}/GetUserById?id=${id}`);
  }

  createUser(user: any): Observable<any> {
    return this.http.post(`${this.apiBaseUrl}/CreateUser`, user);
  }

  updateUser(id: number, user: any): Observable<any> {
    return this.http.put(`${this.apiBaseUrl}/UpdateUser?id=${id}`, user);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiBaseUrl}/DeleteUser?id=${id}`);
  }

  getActiveUsers(): Observable<any> {
    return this.http.get(`${this.apiBaseUrl}/GetActiveUsers`);
  }

  searchUsersByEmail(email: string): Observable<any> {
    return this.http.get(
      `${this.apiBaseUrl}/SearchUsersByEmail?email=${email}`
    );
  }

  activeDeactive(id: number): Observable<any> {
    return this.http.post(`${this.apiBaseUrl}/ActiveDeactive?id=${id}`, {
      id: id,
    });
  }
}
