import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subject,EMPTY } from 'rxjs';
import { catchError, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'geo-history-page',
  templateUrl: './geo-history.component.html',
  styleUrls: ['./geo-history.component.css']
})
export class GeoHistoryPageComponent implements OnInit, OnDestroy {
  public historyItems$: Observable<HistoryItem[]>;

  public errorMessage$: Subject<string> = new Subject<string>();

  private lifeTimeObject: Subject<boolean> = new Subject<boolean>();


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    
  }

  public updateHistory() {
    this.errorMessage$.next(null);

    this.historyItems$ = this.http.get<HistoryItem[]>(this.baseUrl + 'api/geoInfoHistory')
      .pipe(takeUntil(this.lifeTimeObject),
        catchError(this.handleError(this.errorMessage$)));
  }

  private handleError(subject: Subject<string>): (te: any) => Observable<HistoryItem[]> {
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
    this.lifeTimeObject.next(false);
    this.lifeTimeObject.complete();
  }
}

interface HistoryItem {
  id: number;
  dateTimeUtc:string;
  city: string;
  currentTemperatureCelsius: string;
  timeZone:string;
}
