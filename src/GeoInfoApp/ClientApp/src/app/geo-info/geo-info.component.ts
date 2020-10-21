import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subject,EMPTY } from 'rxjs';
import { catchError, takeUntil } from 'rxjs/operators';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';

@Component({
  selector: 'geo-info-page',
  templateUrl: './geo-info.component.html',
  styleUrls: ['./geo-info.component.css']
})
export class GeoInfoPageComponent implements OnInit, OnDestroy {
  public info$: Observable<GeoInfo>;

  public errorMessage$: Subject<string> = new Subject<string>();

  private lifeTimeObject: Subject<boolean> = new Subject<boolean>();

  parametersForm: FormGroup;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private formBuilder: FormBuilder) {
    this.parametersForm = formBuilder.group({
      'zipCode': ['', Validators.pattern(/\d+[,]{1}[a-zA-Z]+/)]
    });
  }

  public submitZip() {

    let zip = this.parametersForm.get('zipCode').value;

    this.info$ = this.http.get<GeoInfo>(this.baseUrl + 'api/geoInfo/' + zip)
      .pipe(takeUntil(this.lifeTimeObject),
        catchError(this.handleError(this.errorMessage$)));
  }

  private handleError(subject: Subject<string>): (te: any) => Observable<GeoInfo> {
    return (error) => {
      let message = '';
      if (error.error instanceof ErrorEvent) {
        message = `Error: ${error.error.message}`;
      } else {
        message = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
      subject.next(message);
      return EMPTY;
    }
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {

  }
}

interface GeoInfo {
  city: string;
  currentTemperatureCelsius: string;
  timeZone:string;
}
