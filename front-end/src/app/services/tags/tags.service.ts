import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';
import { Tag } from '../../models/tag';
import { AccountService } from '../account/account.service';

@Injectable({
  providedIn: 'root'
})
export class TagsService {

  constructor(private httpClient:HttpClient,
  private accService: AccountService) { }

  getAll(): Observable<Tag[]> {
    return this.httpClient.get<Tag[]>('api/tags', {headers:this.accService.getAuthHeaders()});
  }
}