namespace ElevatorSimulation.Domain.Exceptions;

public class UnauthorizedException(string message) : Exception(message);