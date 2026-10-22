#nullable enable

using System;
using DesignPatterns.StrategyPart3;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.DependencyInjectionExamples
{
    /*
     * Vstavljanje odvisnosti (dependency injection).
     * Objekt odvisnosti prejme od zunaj, namesto da bi sam izbiral in ustvarjal njihove implementacije.
     * Tako ločimo sestavljanje objektov od njihove uporabe.      
     *
     * Dokumentacija:
     * https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/overview
     * https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/basics
     * https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/service-lifetimes
     * https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/guidelines
     */
    public static class DependencyInjectionTests
    {
        /// <summary>
        /// Isti razred raziskovalca uporabimo z različnima jezikovnima strategijama.
        /// </summary>
        public static void ShowConstructorInjection()
        {
            // V StrategyPart3.Researcher konstruktor sam izbere implementacijo:
            // SpeakForeign = new SpeakForeignLanguageFluently();
            // Vmesnik zato še ne odpravi neposredne odvisnosti od konkretnega razreda.

            // Pri novi različici Researcher (spodaj) strategijo izberemo tukaj, zunaj raziskovalca.
            IForeignLanguageSpeaker speaker = new SpeakForeignLanguageFluently();
            Researcher researcher = new Researcher("Novak", "Ana", speaker);

            Console.WriteLine($"{researcher.GivenName} {researcher.FamilyName}:");
            researcher.TrySpeakForeignLanguage("angleško");
            // Pričakovano: Tuj jezik angleško govorim tekoče dva dni skupaj!

            // Razreda Researcher ne spreminjamo. Drugemu objektu podamo drugo implementacijo.
            Researcher another = new Researcher("Kos", "Miha", new SpeakForeignLanguageSoSo());

            Console.WriteLine($"{another.GivenName} {another.FamilyName}:");
            another.TrySpeakForeignLanguage("angleško");
            // Pričakovano: Tuj jezik angleško govorim bolj tako tako.

            // Strategy nam da zamenljive implementacije obnašanja.
            // DI pa določa, kako raziskovalec izbrano implementacijo prejme.
        }

        /// <summary>
        /// Storitev za prijave ostane enaka, zamenjamo samo način obveščanja.
        /// </summary>
        public static void ShowNotifiers()
        {
            // 1. Običajna uporaba: obvestilo gre v konzolo.
            INotifier notifier = new ConsoleNotifier();
            RegistrationService registration = new RegistrationService(notifier);

            registration.Register("Ana Novak");
            // Pričakovano: Prijavljen: Ana Novak

            // 2. Preverjanje delovanja: obvestilo shranimo, da lahko preverimo njegovo vsebino.
            // Konkreten tip MemoryNotifier tukaj potrebujemo za dostop do lastnosti LastMessage.
            // RegistrationService pa pozna samo vmesnik INotifier.
            MemoryNotifier memoryNotifier = new MemoryNotifier();
            RegistrationService registrationToCheck = new RegistrationService(memoryNotifier);

            registrationToCheck.Register("Ana Novak");

            Console.WriteLine(memoryNotifier.LastMessage == "Prijavljen: Ana Novak");
            // Pričakovano: True. Preverimo vsebino, ne samo tega, da se koda izvede brez izjeme.

            // Polje notifier v RegistrationService je readonly, zato mu ne moremo zamenjati sklica.
            // Objekt MemoryNotifier pa ni nespremenljiv: njegova funkcija Send spremeni LastMessage.
            // V obeh primerih smo odvisnosti vstavili ročno, brez vsebnika.
        }

        /// <summary>
        /// Vsebnik ustvari RegistrationService in mu sam poda registriranega obveščevalnika.
        /// </summary>
        public static void ShowContainer()
        {
            // Paket Microsoft.Extensions.DependencyInjection je dodan v datoteki projekta.
            // ServiceCollection hrani pravila (registracije), ne že sestavljenega programa.
            ServiceCollection services = new ServiceCollection();

            // Prvi tipski argument je tip storitve, drugi pa razred njene implementacije.
            services.AddTransient<INotifier, ConsoleNotifier>();

            // Za prikaz zamenjave ZGORNJO registracijo nadomestimo z naslednjo:
            // services.AddTransient<INotifier, MemoryNotifier>();
            // Register bo tedaj obvestilo shranil v MemoryNotifier in ga ne bo izpisal.
            // Zaradi Transient bi novo pridobivanje INotifier ustvarilo DRUG obveščevalnik;
            // na njem zato ne bi našli obvestila, poslanega prek spodnje storitve za prijave.

            // Storitev lahko registriramo tudi neposredno s konkretnim tipom, brez svojega vmesnika.
            services.AddTransient<RegistrationService>();

            // Po vseh registracijah zgradimo en vsebnik za ta primer.
            // using ob izhodu iz funkcije pokliče Dispose; to ni ukaz zbiralniku smeti.
            using ServiceProvider provider = services.BuildServiceProvider();

            // Vsebnik prebere javni konstruktor RegistrationService, ustvari ConsoleNotifier,
            // ga poda konstruktorju in vrne sestavljeni objekt RegistrationService.
            RegistrationService registration = provider.GetRequiredService<RegistrationService>();

            registration.Register("Ana Novak");
            // Pričakovano: Prijavljen: Ana Novak

            // Če ima tudi obveščevalnik konstruktorske odvisnosti, vsebnik zagotovi še te.
            // Vsebnik uporabljamo na mestu sestavljanja programa. RegistrationService ga ne pozna,
            // ampak svoje odvisnosti še vedno prejme prek konstruktorja.
        }

        /// <summary>
        /// Isti postopek ponovimo s tremi različnimi registracijami števca.
        /// </summary>
        public static void ShowLifetimes()
        {
            // Pred vsakim primerom napovejmo: ali dobimo isti objekt in kateri vrednosti se izpišeta?
            // Vsak klic spodaj zgradi nov vsebnik, zato primeri ne prenašajo stanja drug v drugega.
            ShowLifetime(ServiceLifetime.Transient);
            ShowLifetime(ServiceLifetime.Scoped);
            ShowLifetime(ServiceLifetime.Singleton);

            // Povzetek:                  isti objekt    prvi obseg    drugi obseg
            // Transient                  False          0             0
            // Scoped                     True           1             0
            // Singleton                  True           1             1
        }

        private static void ShowLifetime(ServiceLifetime lifetime)
        {
            Console.WriteLine($"\nŽivljenjska doba: {lifetime}");
            ServiceCollection services = new ServiceCollection();

            // Za vsak zagon izberemo NATANKO ENO registracijo istega razreda Counter.
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    // Ob vsakem pridobivanju iz vsebnika nastane nova instanca.
                    services.AddTransient<Counter>();
                    break;
                case ServiceLifetime.Scoped:
                    // Znotraj istega obsega uporabljamo isto instanco; drug obseg dobi drugo.
                    services.AddScoped<Counter>();
                    break;
                case ServiceLifetime.Singleton:
                    // Za to registracijo uporabljamo isto instanco v celotnem vsebniku.
                    services.AddSingleton<Counter>();
                    break;
            }

            using ServiceProvider provider = services.BuildServiceProvider(validateScopes: true);

            // Obseg DI ustvari CreateScope, ne samo blok kode med zavitima oklepajema.
            // Storitve pridobivamo iz tega obsega prek scope.ServiceProvider.
            using (IServiceScope scope = provider.CreateScope())
            {
                Counter first = scope.ServiceProvider.GetRequiredService<Counter>();
                Counter second = scope.ServiceProvider.GetRequiredService<Counter>();

                first.Increment();

                Console.WriteLine($"Isti objekt: {ReferenceEquals(first, second)}");
                Console.WriteLine($"Prvi obseg, second.Count: {second.Count}");

                // Pri Transient se spremeni samo first. Pri Scoped in Singleton sta sklica na isti objekt.
                // Ponoven klic first.Increment() ne bi ustvaril novega objekta niti pri Transient.
            }

            // Prvi obseg se je zaključil. V novem obsegu zahtevamo še en števec.
            using (IServiceScope scope = provider.CreateScope())
            {
                Counter third = scope.ServiceProvider.GetRequiredService<Counter>();
                Console.WriteLine($"Drugi obseg, third.Count: {third.Count}");

                // Pri Scoped dobimo nov števec, zato je Count enak 0.
                // Pri Singleton dobimo isti števec kot prej, zato je Count enak 1.
            }

            // AddSingleton ne zahteva zasebnega konstruktorja ali statične lastnosti Counter.Instance.
            // Ponovno uporabo zagotavlja vsebnik. Z new Counter() bi lahko ustvarili še drug objekt.
            // Storitev Singleton zaradi svoje registracije ni samodejno varna za hkratno uporabo iz več niti.

            // Vsebnik zaključi uporabo svojih storitev IDisposable glede na njihovo življenjsko dobo.
            // Naš Counter ne upravlja takih virov in ne potrebuje lastne funkcije Dispose.
        }

        /// <summary>
        /// Prikažemo dve pogosti napaki pri sestavljanju storitev in razložimo njun vzrok.
        /// </summary>
        public static void ShowErrors()
        {
            Console.WriteLine("1. Manjkajoča registracija obveščevalnika");
            {
                ServiceCollection services = new ServiceCollection();
                services.AddTransient<RegistrationService>();
                // NAMERNO manjka: services.AddTransient<INotifier, ConsoleNotifier>();
                using ServiceProvider provider = services.BuildServiceProvider();

                try
                {
                    // RegistrationService je registriran, toda njegove odvisnosti ni mogoče zagotoviti.
                    provider.GetRequiredService<RegistrationService>();
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine("Pričakovana napaka: manjka registracija INotifier.");
                    Console.WriteLine(exception.Message);
                }

                // Popravek: pred BuildServiceProvider registriramo tudi implementacijo INotifier.
            }

            Console.WriteLine("\n2. Singleton neposredno zahteva storitev Scoped");
            {
                ServiceCollection services = new ServiceCollection();
                services.AddScoped<Counter>();
                services.AddSingleton<SingletonCounterConsumer>();

                // S preverjanjem obsegov to nepravilnost zaznamo ob pridobivanju storitve.
                using ServiceProvider provider = services.BuildServiceProvider(validateScopes: true);
                using IServiceScope scope = provider.CreateScope();

                try
                {
                    // Tudi pridobivanje znotraj obsega ne popravi napačne odvisnosti singletona.
                    scope.ServiceProvider.GetRequiredService<SingletonCounterConsumer>();
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine("Pričakovana napaka: singleton ne sme neposredno zadržati števca Scoped.");
                    Console.WriteLine(exception.Message);
                }

                // Če odjemalec pripada posameznemu obsegu, ga registriramo z AddScoped.
                // Ime razreda nato prilagodimo novi vlogi. Preverjanja ne izklapljamo kot 'popravek'.
            }
        }
    }

    /// <summary>
    /// Naslednja različica raziskovalca: jezikovno strategijo prejme od zunaj.
    /// Ponovno uporabimo Employee, IForeignLanguageSpeaker in implementacije iz StrategyPart3.
    /// </summary>
    public class Researcher : Employee
    {
        public Researcher(string familyName, string givenName, IForeignLanguageSpeaker speaker)
            : base(familyName, givenName)
        {
            ArgumentNullException.ThrowIfNull(speaker);

            // Ta prireditev je bistvo spremembe iz zapiskov: ni več new SpeakForeignLanguageFluently().
            SpeakForeign = speaker;

            // Obstoječi Employee ima še akademske in izpitne funkcionalnosti.
            // Ohranimo njuni implementaciji iz prejšnjega primera, da sta tudi ti funkcionalnosti uporabni.
            // V tem zgledu vstavljamo samo jezikovno strategijo; enako bi lahko podali tudi preostali dve.
            AcademicPerformer = new AcademicInNaturalSciences();
            Examination = new ExaminationPerson();
        }
    }

    /// <summary>
    /// Pogodba za obveščanje; uporabnik vmesnika ne potrebuje podatka o načinu pošiljanja.
    /// </summary>
    public interface INotifier
    {
        void Send(string message);
    }

    /// <summary>
    /// Obvestilo izpiše v konzolo.
    /// </summary>
    public class ConsoleNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine(message);
        }
    }

    /// <summary>
    /// Shrani zadnje obvestilo, da lahko njegovo vsebino uporabimo pri preverjanju delovanja.
    /// </summary>
    public class MemoryNotifier : INotifier
    {
        public string LastMessage { get; private set; } = "";

        public void Send(string message)
        {
            LastMessage = message;
        }
    }

    /// <summary>
    /// Pripravi obvestilo o prijavi. Primer ne implementira shranjevanja ali validacije prijav.
    /// </summary>
    public class RegistrationService
    {
        // readonly preprečuje poznejšo zamenjavo sklica, ne pa sprememb v samem obveščevalniku.
        private readonly INotifier notifier;

        public RegistrationService(INotifier notifier)
        {
            ArgumentNullException.ThrowIfNull(notifier);
            this.notifier = notifier;
        }

        public void Register(string name)
        {
            // Ne izbiramo ConsoleNotifier ali MemoryNotifier in ne kličemo vsebnika.
            // Uporabimo storitev, ki nam jo je podal klicatelj konstruktorja.
            notifier.Send("Prijavljen: " + name);
        }
    }

    /// <summary>
    /// Preprost objekt s stanjem, na katerem opazujemo ponovno uporabo instance.
    /// </summary>
    public class Counter
    {
        // Vsaka nova instanca začne z 0; polje v ozadju ni statično.
        public int Count { get; private set; }

        public void Increment()
        {
            Count++;
        }
    }

    /// <summary>
    /// Odjemalec za prikaz napačne kombinacije Singleton -> Scoped.
    /// Življenjsko dobo mu določi registracija v ShowErrors, ne ime razreda.
    /// </summary>
    public class SingletonCounterConsumer
    {
        public Counter Counter { get; }

        public SingletonCounterConsumer(Counter counter)
        {
            Counter = counter;
        }
    }
}

