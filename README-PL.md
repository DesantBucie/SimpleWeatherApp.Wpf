# Dokumentacja projektu

## Wybrany temat: Aplikacja pokazująca pogodę w zależności od wybranej lokacji

### Użyte API

Aplikacja używa dwóch API.

1. Pierwsze z nich to open-meteo.com. Wysyłane jest do niego szerokość i długość geograficzna, a zwracany jest dość skomplikowany(i w zależności od opcji w URL) obiekt JSON który jest odwzorowany klasą do której następuje deserializacja z JSONa. Pola muszą mieć dokładnie takie same nazwy jak w odpowiedzi JSON, inaczej nie zostaną zapisane do programu.

1. Drugie z nich to https://photon.komoot.io. Służy do przekonwertowania nazwy miejsca na współrzędne geograficzne, które wysyłam do open-meteo.com. Na ten moment(mogło ulec zmianie od pisania tego), niestety nie wyświetla się lista miejsc o tej samej nazwie a różnych. Kiedy wpiszemy Moscow, nie wyświetli się więc Moskwa, stolica Rosji, tylko jakieś miasteczko w USA. Musimy wpisać Moskva, a więc angielską nazwę.

### Działanie

Aplikacja ma zaimplementowane pobieranie i przypisywanie danych z API w projekcie BLL, który jest biblioteką klas WPF. Za pobieranie danych odpowiada systemowa biblioteka HTTP(using System.Net.Http), za deserializację JSONa, biblioteka od NewtonSoftu, ponieważ systemowa miała problemy z tablicami. Klasy Weather is GeoCoding(oraz ich pochodne) są wymodelowanea ich pola nazwane według zwracanego obiektu typu JSON.

Zasada jest prosta. Jeżeli interesuje nas pewne pole z JSONa to tworzymy pole o takiej samej nazwie(także wielkość liter ma znaczenie), a podczas deserializacji funkcja automatycznie przypisze pola o tej samej nazwie.




