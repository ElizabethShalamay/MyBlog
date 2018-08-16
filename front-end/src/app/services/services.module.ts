import { NgModule } from '@angular/core';
import { AccountService } from './account/account.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    HttpClientModule
  ],
  providers: [
    AccountService
  ]
})
export class ServicesModule {}
