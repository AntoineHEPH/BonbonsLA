using Condorcet.B2.AspnetCore.MVC.Application.Core.Domain;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;
using Condorcet.B2.AspnetCore.MVC.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Condorcet.B2.AspnetCore.MVC.Application.Controllers;

public class CartController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;

    public CartController(IUserRepository userRepository, ICartRepository cartRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    private async Task<User?> GetCurrentUser()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return null;

        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task<IActionResult> Index()
    {
        var user = await GetCurrentUser();

        if (user == null)
            return RedirectToAction("Login", "Account");

        var products = await _cartRepository.GetAll(user.Id);

        var viewModel = new CartIndexViewModel
        {
            Products = products
        };

        return View(viewModel);
    }

}