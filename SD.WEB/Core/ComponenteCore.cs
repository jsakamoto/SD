﻿using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SD.WEB.Modules.Auth.Core;

namespace SD.WEB.Core
{
    public static class ComponenteUtils
    {
        private static string? idUser;

        public static string GetIdUser(bool authenticating = false)
        {
            if (authenticating)
            {
                return idUser ?? string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(idUser)) throw new InvalidOperationException("User Not Defined!");

                return idUser;
            }
        }

        public static void SetIdUser(string? value)
        {
            idUser = value;
        }

        public static bool IsAuthenticated { get; set; }
    }

    /// <summary>
    /// if you implement the OnInitializedAsync method, call 'await base.OnInitializedAsync();'
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    public abstract class ComponenteCore<TClass> : ComponentBase where TClass : class
    {
        [Inject] protected ILogger<TClass> Logger { get; set; } = default!;
        [Inject] protected INotificationService Toast { get; set; } = default!;
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject] protected HttpClient Http { get; set; } = default!; //todo: move to PageCore

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(ComponenteUtils.GetIdUser(authenticating: true)))
                {
                    var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    var user = authState.User;

                    ComponenteUtils.IsAuthenticated = user.Identity != null && user.Identity.IsAuthenticated;
                    ComponenteUtils.SetIdUser(user.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                }
            }
            catch (Exception ex)
            {
                ex.ProcessException(Toast, Logger);
            }
        }
    }

    /// <summary>
    /// if you implement the OnInitializedAsync method, call 'await base.OnInitializedAsync();'
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PageCore<T> : ComponenteCore<T> where T : class
    {
        [Inject] protected IJSRuntime JsRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected PrincipalApi PrincipalApi { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (ComponenteUtils.IsAuthenticated)
                {
                    var principal = await PrincipalApi.Get();

                    //força o cadastro, caso não tenha registrado a conta principal
                    if (principal == null)
                    {
                        Navigation.NavigateTo("/ProfilePrincipal");
                    }
                }

                await base.OnInitializedAsync();

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.ProcessException(Toast, Logger);
            }
        }

        protected abstract Task LoadData();
    }
}