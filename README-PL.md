# Dokumentacja projektu

## Wybrany temat: Aplikacja pokazująca pogodę w zależności od wybranej lokacji

## Użyte API

Aplikacja używa dwóch API.

Pierwsze z nich to open-meteo.com. Wysyłane jest do niego szerokość i długość geograficzna, a zwracany jest dość skomplikowany(i w zależności od opcji w URL) obiekt JSON który jest odwzorowany klasą do której następuje deserializacja z JSONa. Pola muszą mieć dokładnie takie same nazwy jak w odpowiedzi JSON, inaczej nie zostaną zapisane do programu.

Drugie z nich to [TODO]

## WPF i BLL

Aplikacja ma zaimplementowane pobieranie i przypisywanie danych z API w projekcie BLL, który jest biblioteką klas WPF.
