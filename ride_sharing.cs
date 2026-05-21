using System;

namespace FareEngineAssessment
{
    public enum TripStatus { Pending, Paid, Failed }

    public interface IPromotion{
        decimal ApplyDiscount(decimal currentFare);
    }

    public interface IPaymentService{
        bool ProcessPayment(string passengerId, decimal amount);
    }

    public abstract class Vehicle{
        public string LicensePlate { get; private set; }

        public decimal BaseFare { get; private set; }

        public Vehicle(string plate, decimal fare){
            LicensePlate = plate;
            BaseFare = fare;
        }

        public abstract decimal GetPerKmRate();

        public abstract decimal GetPerMinuteRate();

        public virtual decimal GetExtraCharge(){
            return 0;
        }
    }

    public class StandardCar : Vehicle
    {
        private decimal perKmRate = 15;

        private decimal perMinuteRate = 2;

        public StandardCar(string plate, decimal fare) : base(plate, fare){
            
        }

        public override decimal GetPerKmRate(){
            return perKmRate;
        }

        public override decimal GetPerMinuteRate(){
            return perMinuteRate;
        }
    }

    public class LuxurySedan : Vehicle
    {
        private decimal perKmRate = 30;

        private decimal perMinuteRate = 5;

        private decimal luxuryTax = 100;

        public LuxurySedan(string plate, decimal fare) : base(plate, fare){
            
        }

        public override decimal GetPerKmRate(){
            return perKmRate;
        }

        public override decimal GetPerMinuteRate(){
            return perMinuteRate;
        }

        public override decimal GetExtraCharge(){
            return luxuryTax;
        }
    }

    public class Passenger
    {
        public string PassengerId { get; set; }

        public string Name { get; set; }

        public Passenger(string id, string name){
            PassengerId = id;
            Name = name;
        }
    }

    public class PercentageDiscount : IPromotion
    {
        public decimal Percentage { get; set; }

        public PercentageDiscount(decimal percentage){
            Percentage = percentage;
        }

        public decimal ApplyDiscount(decimal currentFare){
            decimal discountAmount = currentFare * Percentage / 100;
            return currentFare - discountAmount;
        }
    }

    public class FlatDiscount : IPromotion
    {
        private decimal minimumFare = 0;

        public decimal Amount { get; set; }

        public FlatDiscount(decimal amount){
            Amount = amount;
        }

        public decimal ApplyDiscount(decimal currentFare)
        {
            decimal discountedFare = currentFare - Amount;

            if(discountedFare < minimumFare){
                discountedFare = minimumFare;
            }

            return discountedFare;
        }
    }

    public class CreditCardPaymentService : IPaymentService
    {
        public bool ProcessPayment(string passengerId, decimal amount){
            Console.WriteLine("Processing Payment...");
            Console.WriteLine("Passenger ID: " + passengerId);
            Console.WriteLine("Amount: " + amount);

            return true;
        }
    }

    public class InvalidTripException : Exception
    {
        public InvalidTripException(string message) : base(message){
            
        }
    }

    public class Trip
    {
        public Vehicle vehicle;

        public Passenger passenger;

        public double DistanceKms;

        public int DurationMinutes;

        public IPromotion promotion;

        public TripStatus Status { get; set; }

        public Trip(
            Vehicle v,
            Passenger p,
            double distance,
            int duration,
            IPromotion promo = null
        )
        {
            if(distance < 0){
                throw new InvalidTripException("Distance cannot be negative");
            }

            if(duration <= 0){
                throw new InvalidTripException("Duration cannot be zero");
            }

            vehicle = v;
            passenger = p;
            DistanceKms = distance;
            DurationMinutes = duration;
            promotion = promo;

            Status = TripStatus.Pending;
        }

        public decimal CalculateFinalFare()
        {
            if(vehicle == null){
                throw new InvalidOperationException("Vehicle not assigned");
            }

            decimal distanceFare = (decimal)DistanceKms * vehicle.GetPerKmRate();

            decimal timeFare = DurationMinutes * vehicle.GetPerMinuteRate();

            decimal extraCharge = vehicle.GetExtraCharge();

            decimal totalFare = vehicle.BaseFare + distanceFare + timeFare + extraCharge;

            // Console.WriteLine(totalFare);

            if(promotion != null){
                totalFare = promotion.ApplyDiscount(totalFare);

                if(totalFare < vehicle.BaseFare){
                    totalFare = vehicle.BaseFare;
                }
            }

            return totalFare;
        }

        public void CompleteTrip(IPaymentService paymentService)
        {
            decimal finalFare = CalculateFinalFare();

            bool paymentSuccess = paymentService.ProcessPayment(passenger.PassengerId, finalFare);

            if(paymentSuccess){
                Status = TripStatus.Paid;
            }
            else{
                Status = TripStatus.Failed;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Passenger passenger1 = new Passenger("P101", "Rahim");

            Vehicle car1 = new StandardCar("DHA-123", 50);

            IPromotion promo1 = new PercentageDiscount(10);

            Trip trip1 = new Trip(car1, passenger1, 10, 20, promo1);

            decimal fare1 = trip1.CalculateFinalFare();

            Console.WriteLine("Standard Car Fare: " + fare1);

            Console.WriteLine();

            trip1.CompleteTrip(new CreditCardPaymentService());

            Console.WriteLine("Trip Status: " + trip1.Status);

            Console.WriteLine();

            Vehicle car2 = new LuxurySedan("DHA-999", 100);

            IPromotion promo2 = new FlatDiscount(200);

            Trip trip2 = new Trip(car2, passenger1, 5, 15, promo2);

            decimal fare2 = trip2.CalculateFinalFare();

            Console.WriteLine("Luxury Sedan Fare: " + fare2);

            Console.WriteLine();

            trip2.CompleteTrip(new CreditCardPaymentService());

            Console.WriteLine("Trip Status: " + trip2.Status);

            Console.WriteLine();

            for(int i = 1; i <= 3; i++){
                Console.WriteLine("Trip Number: " + i);
            }

            Console.WriteLine();

            int rating = 5;

            if(rating >= 4){
                Console.WriteLine("Good Ride");
            }
            else{
                Console.WriteLine("Bad Ride");
            }

            // Console.WriteLine(fare1);
        }
    }
}